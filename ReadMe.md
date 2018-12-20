# Our current situation is: to fix `bug2` we need to upgrade to `9.6.646`, but in `9.6.646` we have `bug1`.


# Bug1: Devart.Data.Oracle.OracleException (0x80004005): ORA-00936: missing expression
This happens in `9.6.646`.

Reproduce steps:
1. Change devart version to `9.6.646` in EFCoreConcurrentIssue.csproj
2. Cd to EFCoreConcurrentIssue, run: `dotnet run`
3. In the browser access: http://localhost:5000/api/values/test2


# Bug2: The issue is related to `HasQueryFilter` in the MyDbContext.cs with devar.data.oracle.efcore provider version `9.6.597`
This only hanpens with devart.data.oracle.efcore provider, I test with other providers all works fine. And it works fine in `9.6.646` now.

We use this for soft deleting stuff. 

Repoduce steps:
1. Change devart version to `9.6.597` in EFCoreConcurrentIssue.csproj
2. Config the database connection with devart license
3. Cd to EFCoreConcurrentIssue, run: `dotnet run`
4. Wait for db creating
5. Cd to TestConsole folder run： `dotnet run`
6. Monitor the ouput from the EFCoreConcurrentIssue console app, you will see `System.InvalidOperationException: Collection was modified; enumeration operation may not execute.`

   If you cannot reproduce, then go to TestConsole Program.fs and increase `concurrentCount`, then try again. 
