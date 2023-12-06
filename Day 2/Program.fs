open System.IO
open System.Collections.Generic

type Set = { Green: int; Blue: int; Red: int }
type Game = { Id: int; Sets: IEnumerable<Set> }

let ParseGameId (line: string) =
    match line.Split(":") with
    | [| title; _ |] ->
        match title.Split(" ") with
        | [| _; id |] -> int (id)
        | _ -> failwith "Line with no \"Game x\""
    | _ -> failwith "Line with no \":\""


let ParseColor (color: string, cubes: string array) =
    let colorCube = cubes |> Seq.tryFind _.Contains(color)

    match colorCube with
    | Some(s) ->
        match s.Trim().Split(" ") with
        | [| count; _ |] -> int (count)
        | _ -> failwith "We should have \"{count} {color}\""
    | None -> 0

let ParseSet (line: string) =
    let cubes = line.Split(",")

    { Green = ParseColor("green", cubes)
      Blue = ParseColor("blue", cubes)
      Red = ParseColor("red", cubes) }

let ParseSets (line: string) =
    match line.Split(":") with
    | [| _; sets |] -> sets.Trim().Split(";") |> Seq.map ParseSet
    | _ -> failwith "Theres should only be two elements"

let parseGame (line: string) =
    { Id = ParseGameId(line)
      Sets = ParseSets(line) }

let inputFile = "input.txt"

let isValidSet (s: Set) =
    s.Red <= 12 && s.Green <= 13 && s.Blue <= 14

let games = File.ReadAllLines(inputFile) |> Seq.map parseGame

let sumOfGameIdsWithValidSets =
    games |> Seq.filter (fun g -> g.Sets |> Seq.forall isValidSet) |> Seq.sumBy _.Id

printfn "%i" sumOfGameIdsWithValidSets


let powers =
    games
    |> Seq.sumBy (fun g ->
        let maxRed = g.Sets |> Seq.map _.Red |> Seq.max
        let maxGreen = g.Sets |> Seq.map _.Green |> Seq.max
        let maxBlue = g.Sets |> Seq.map _.Blue |> Seq.max

        maxRed * maxGreen * maxBlue)

printfn "%i" powers
