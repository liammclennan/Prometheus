namespace MyModels.Something
    
    [<CLIMutable>]
    type AjaxOptions = { ``Type``: string; Url: string; DataType: string }


    [<CLIMutable>]
    type Address = { number: int; street: string }

    [<CLIMutable>]
    type Person = { name: string; age: int; address: Address }