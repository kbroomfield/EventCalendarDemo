# EventCalendar
Event Calendar API for demo

# To Build and Run
Must have Visual Studio 2017 and ASP.NET MVC Core tooling.

Open a command prompt in the project (or open the solution in VS and use the Package Manager Console) and run `dotnet restore` to make sure that all the required packages are restored to the project. If for some reason NuGet doesn't pick up NuGet.Config and you get errors saying that it wasn't able to install some packages, add the following to your NuGet package sources:

`name: aspnet-contrib source: https://www.myget.org/F/aspnet-contrib/api/v3/index.json`

Run `dotnet restore` again to install the required packages.
