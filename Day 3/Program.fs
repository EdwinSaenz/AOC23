open System
open System.IO
open System.Text.RegularExpressions

let inputFile = "input.txt"

let isSymbol c = not <| ((c = '.') || Char.IsDigit c)

let lines = File.ReadAllLines(inputFile)

[| 0 .. (lines.Length - 1) |]
|> Seq.map (fun currentRow ->
    let line = lines[currentRow]
    let matches = Regex.Matches(line, "\\d+")

    matches
    |> Seq.map (fun m ->
        let row = if currentRow = 0 then 0 else currentRow - 1

        let lastRow =
            if currentRow = lines.Length - 1 then
                lines.Length - 1
            else
                currentRow + 1

        let result =
            [| row..lastRow |]
            |> Seq.exists (fun r ->
                let col = if m.Index = 0 then 0 else m.Index - 1

                let lastCol =
                    if m.Index + m.Length = line.Length then
                        line.Length - 1
                    else
                        m.Index + m.Length

                [ col..lastCol ] |> Seq.exists (fun c -> isSymbol (lines[r][c])))

        if result then int m.Value else 0)
    |> Seq.sum)
|> Seq.sum
|> printfn "%i"
