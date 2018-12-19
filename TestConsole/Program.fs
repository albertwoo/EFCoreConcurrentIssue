open System
open System.Net.Http

let getTest (httpClient: HttpClient) _ =
    async {
        printf "o"
        try
            let! resp = httpClient.GetAsync("http://localhost:5000/api/values") |> Async.AwaitTask
            let! content = resp.Content.ReadAsStringAsync() |> Async.AwaitTask
            match int resp.StatusCode with
            | 500 -> printfn "%s" content
            | _ -> printf "x"
        with
        | _ -> ()
    }


[<EntryPoint>]
let main argv =
    let httpClient = new HttpClient()
    
    [0..1000]
    |> Seq.map (getTest httpClient)
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore


    0 // return an integer exit code
