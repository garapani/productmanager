# productmanager

This project is build using C# and .net core framework.  <br/> <br/>
**Steps to run the project:** <br/>
1. On the command prompt, and then go to folder _ProductManager_. <br/>
2. Run the below commands <br/>
<code>dotnet restore</code> <br/>
<code>dotnet run</code> <br/>
3. after these commands, open the brower with the url <a href="https://localhost:5001/swagger/index.html">https://localhost:5001/swagger/index.html</a>, swagger document opens with avaialble REST APIs.

**Steps to run the integration tests:** <br/>
To run the automation tests, first need to delete the DB file (database.sqlite) file present under the _data_ folder (since the project and automation test using the same db). <br/>
1. Open the command prompt and go to folder '_ProductManager.IntegrationTests_' <br/>
2. run the below commands
  <code>dotnet restore</code> <br/>
  <code>dotnet test</code> <br/>
3. In the command prompts it will show the test results.
