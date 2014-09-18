﻿open System.Reflection
open Microsoft.FSharp.Reflection

let private convertType fn : string = 
    match fn with
        | "String" -> "string"
        | "Int32" -> "number"
        | _ -> fn

let fieldToCtorDef (f:PropertyInfo) : string =
    sprintf "public %s: %s" f.Name (convertType f.PropertyType.Name)

let convert t : string =
    let fields = FSharpType.GetRecordFields t
    let ctorDef = System.String.Join(", ", fields |> Array.map fieldToCtorDef)
    let wrapNs cls = if t.Namespace <> null && t.Namespace <> "" then (t.Namespace, cls) ||> sprintf "module %s { %s\n}"  else cls
    sprintf """
export class %s {
    constructor(%s) {}
}""" t.Name ctorDef
        |> wrapNs

let convertAssembly (a:Assembly) =
    let types = a.GetTypes()
    let records = types |> Array.filter Reflection.FSharpType.IsRecord
    let typeScriptClassDefs = records |> Array.map convert
    String.concat "\n" typeScriptClassDefs

[<EntryPoint>]
let main argv = 
    convertAssembly (Assembly.GetExecutingAssembly()) 
        |> printfn "%s"
    0 // return an integer exit code