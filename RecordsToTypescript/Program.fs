open System.Reflection
open System.IO
open Prometheus

let getResult fn =
    try
        fn
            |> System.IO.Path.GetFullPath 
            |> Assembly.LoadFrom
            |> convertAssembly 
            |> Choice1Of2 
    with 
        | :? System.Reflection.ReflectionTypeLoadException as ex ->
            (fn, ex.LoaderExceptions) 
                ||> sprintf "Failed to load %s. Message: %A" 
                |> Choice2Of2
        | ex -> 
            (fn, ex) 
                ||> sprintf "Failed to load %s. Message: %A" 
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