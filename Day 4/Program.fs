open System
open System.IO
open System.Text.RegularExpressions

let inputFile = "input.txt"
let lines = File.ReadAllLines(inputFile)

// Part 1
lines
|> Seq.map (fun line ->
    match line.Split(":") with
    | [| _; numbers |] ->
        match numbers.Split("|") with
        | [| left; right |] ->
            let winningMatches = Regex.Matches(left, "\\d+")
            let chosenMatches = Regex.Matches(right, "\\d+")

            let cardScore =
                chosenMatches
                |> Seq.filter (fun chosenMatch ->
                    winningMatches
                    |> Seq.exists (fun winningMatch -> chosenMatch.Value = winningMatch.Value))
                |> Seq.fold (fun score _ -> score * 2) 1

            cardScore / 2
        | _ -> failwith "{winning} | {chosen} format not matched"
    | _ -> failwith "Card x : {numbers} format not matched")
|> Seq.sum
|> printfn "%i"


// Part 2
[| 0 .. lines.Length - 1 |]
|> Seq.fold
    (fun (instances: int array) cardIndex ->
        let line = lines[cardIndex]

        match line.Split(":") with
        | [| _; numbers |] ->
            match numbers.Split("|") with
            | [| left; right |] ->
                let winningMatches = Regex.Matches(left, "\\d+")
                let chosenMatches = Regex.Matches(right, "\\d+")

                let matchingNumbers =
                    chosenMatches
                    |> Seq.filter (fun chosenMatch ->
                        winningMatches
                        |> Seq.exists (fun winningMatch -> chosenMatch.Value = winningMatch.Value))
                    |> Seq.length

                let head = instances |> Seq.take (cardIndex + 1)

                let updatedInstances =
                    [ cardIndex + 1 .. cardIndex + matchingNumbers ]
                    |> Seq.map (fun i -> instances[i] + instances[cardIndex])

                let tail = instances |> Seq.skip (cardIndex + matchingNumbers + 1)

                tail |> Seq.append updatedInstances |> Seq.append head |> Seq.toArray

            | _ -> failwith "{winning} | {chosen} format not matched"
        | _ -> failwith "Card x : {numbers} format not matched")
    (Array.create lines.Length 1)
|> Seq.sum
|> printfn "%i"
