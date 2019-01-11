open System
open System.Net.Http
open System.Diagnostics

    
let concurrentCount = 50


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

    let sw = Stopwatch()
    sw.Start()
    
    [0..concurrentCount]
    |> Seq.iter ((getTest httpClient) >> Async.RunSynchronously)
    //|> Async.Parallel
    //|> Async.RunSynchronously
    |> ignore

    sw.Stop()
    printfn "Finished time: %d ms" sw.ElapsedMilliseconds


    0 // return an integer exit code
