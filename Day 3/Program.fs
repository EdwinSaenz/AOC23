open System
open System.IO
open System.Text.RegularExpressions

let inputFile = "input.txt"
let lines = File.ReadAllLines(inputFile)

// Part 1
[| 0 .. (lines.Length - 1) |]
|> Seq.map (fun currentRow ->
    let line = lines[currentRow]
    let matches = Regex.Matches(line, "\\d+")

    matches
    |> Seq.map (fun m ->
        let rowsToScan =
            match currentRow with
            | 0 -> [| currentRow; currentRow + 1 |]
            | r when r + 1 = lines.Length -> [| currentRow - 1; currentRow |]
            | _ -> [| currentRow - 1; currentRow; currentRow + 1 |]

        let result =
            rowsToScan
            |> Seq.exists (fun row ->
                let colsToScan =
                    match m.Index with
                    | 0 ->
                        if row = currentRow then
                            [| m.Length |]
                        else
                            [| 0 .. m.Length |]
                    | c when c + m.Length = line.Length ->
                        if row = currentRow then
                            [| m.Index - 1 |]
                        else
                            [| m.Index - 1 .. line.Length - 1 |]
                    | _ ->
                        if row = currentRow then
                            [| m.Index - 1; m.Index + m.Length |]
                        else
                            [| m.Index - 1 .. m.Index + m.Length |]

                let isSymbol c = not <| ((c = '.') || Char.IsDigit c)
                colsToScan |> Seq.exists (fun col -> isSymbol <| lines[row][col]))

        if result then int m.Value else 0)
    |> Seq.sum)
|> Seq.sum
|> printfn "%i"


// Part 2
[| 0 .. (lines.Length - 1) |]
|> Seq.map (fun currentRow ->
    let line = lines[currentRow]
    let matches = Regex.Matches(line, "\\*")

    matches
    |> Seq.map (fun m ->
        let rowsToScan =
            match currentRow with
            | 0 -> [| currentRow; currentRow + 1 |]
            | r when r + 1 = lines.Length -> [| currentRow - 1; currentRow |]
            | _ -> [| currentRow - 1; currentRow; currentRow + 1 |]

        let numbers =
            rowsToScan
            |> Seq.fold
                (fun state row ->
                    let numbers =
                        Regex.Matches(lines[row], "\\d+")
                        |> Seq.filter (fun numberMatch ->
                            m.Index <= numberMatch.Index + numberMatch.Length
                            && m.Index >= numberMatch.Index - 1)
                        |> Seq.map (fun numberMatch -> int numberMatch.Value)
                        |> Seq.toList


                    state @ numbers)
                List.empty

        if numbers.Length = 2 then numbers[0] * numbers[1] else 0)
    |> Seq.sum)
|> Seq.sum
|> printfn "%i"
