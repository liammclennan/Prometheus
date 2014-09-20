open System.Reflection
open System.IO
open Fire

let getResult fn =
    try
        fn
            |> System.IO.Path.GetFullPath 
            |> Assembly.LoadFile 
            |> convertAssembly 
            |> Choice1Of2 
    with 
        | ex -> 
            (fn, ex.Message) 
                ||> sprintf "Failed to load %s. Message: %s" 
                |> Choice2Of2

[<EntryPoint>]
let main argv =
    if Array.length argv <> 1 then 
        printfn "Usage: Prometheus <assemblyname>"
        1
    else
        match getResult argv.[0] with
            | Choice1Of2 v -> 
                let filename = Path.GetFileNameWithoutExtension(argv.[0]) 
                                |> sprintf "%s_types.ts"
                File.WriteAllText(filename, v)
                (Path.GetFileName(argv.[0]), filename) ||> printfn "All record types in assembly %s written to file %s"
                0 // return an integer exit code
            | Choice2Of2 e ->
                printfn "%s" e
                1