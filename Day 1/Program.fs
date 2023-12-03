open System
open System.IO

let fileName = "input.txt"

File.ReadAllLines fileName
|> Seq.map (fun line ->
    let first = line |> Seq.find Char.IsDigit
    let last = line |> Seq.findBack Char.IsDigit
    Int32.Parse $"{first}{last}")
|> Seq.sum
|> printfn "answer 1: %i"

let wordNumbers =
    [ ("one", "1")
      ("two", "2")
      ("three", "3")
      ("four", "4")
      ("five", "5")
      ("six", "6")
      ("seven", "7")
      ("eight", "8")
      ("nine", "9") ]

let rec replaceWordsWithNumbers (line: String) =
    let ocurrances =
        wordNumbers
        |> Seq.map (fun (key, value) -> (line.IndexOf(key), key, value))
        |> Seq.where (fun (index, _, _) -> index > -1)

    if Seq.isEmpty ocurrances then
        line
    else
        let (index, _, value) = ocurrances |> Seq.minBy (fun (index, _, _) -> index)

        line.Remove(index) + value + line.Substring(index + 1)
        |> replaceWordsWithNumbers

File.ReadAllLines fileName
|> Seq.map (fun line ->
    let newLine = replaceWordsWithNumbers line
    let first = newLine |> Seq.find Char.IsDigit
    let last = newLine |> Seq.findBack Char.IsDigit
    Int32.Parse $"{first}{last}")
|> Seq.sum
|> printfn "answer 2: %i"
