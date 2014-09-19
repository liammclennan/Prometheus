open System.Reflection
open System.IO
open Fire

[<EntryPoint>]
let main argv =
    if Array.length argv <> 1 then 
        printfn "Usage: Prometheus <assemblyname>"
        1
    else
        argv.[0] 
            |> System.IO.Path.GetFullPath 
            |> Assembly.LoadFile 
            |> convertAssembly 
            |> printfn "%s"
        0 // return an integer exit code
