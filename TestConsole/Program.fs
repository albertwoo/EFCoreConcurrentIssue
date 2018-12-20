open System
open System.Net.Http

    
let concurrentCount = 1000


let getTest (httpClient: HttpClient) _ =
    async {
        printf "o"
        try
            let! resp = httpClient.GetAsync("http://localhost:5000/api/values") |> Async.AwaitTask
            match int resp.StatusCode with
            | 500 -> printfn "e"
            | _ -> printf "x"
        with
        | _ -> ()
    }


[<EntryPoint>]
let main argv =
    let httpClient = new HttpClient()

    [0..concurrentCount]
    |> Seq.map (getTest httpClient)
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore

    0 // return an integer exit code
