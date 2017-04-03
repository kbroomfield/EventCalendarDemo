# EventCalendar
Event Calendar API for demo

# To Build and Run
Must have Visual Studio 2017 and ASP.NET MVC Core tooling.

Open the solution in Visual Studio. Then, open the Package Manager Console and run `dotnet restore` to make sure that all the required packages are restored to the project. If for some reason NuGet doesn't pick up NuGet.Config and you get errors saying that it wasn't able to install some packages, add the following to your NuGet package sources:

`name: aspnet-contrib 
source: https://www.myget.org/F/aspnet-contrib/api/v3/index.json`

Run `dotnet restore` again to install the required packages.

Choose the `Start Without Debugging` option from the Debug menu to run the program.

# Misc
This application is an attempt to meet a certain number of requirements in a 60 to 90 minute time frame. I spent approximately 100 minutes building this solution. I didn't get nearly as far as I would have liked, but I believe it is a good start.
