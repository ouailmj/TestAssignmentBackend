# TestAssignmentBackend

## Instructions

We would like to implement an auto-complete feature for movies with a learn-to-rank (LTR) mechanism to enhance the relevance of the auto-complete by user clicks on the results shown for a particular search.

### Requirements

- Deploy Elasticsearch and populate it with movie data from [here](https://grouplens.org/datasets/movielens/1m/). The document structure should have MovieName, Year, Genre(s).

- Track users' search/views/clicks and store them in another database.

- Create a UI for the search input and showing the results as the user types (auto-complete). Results should contain the entire movie info.

- Use ASP.NET Core as an API for Elasticsearch and for the LTR implementation.

- Demonstrate how relevant search results are enhanced with more clicks on some results from a particular search.

- The search should show results even if the words are misspelled or stemming (plural/singular handling).

# Technical documentation

- This is a .NET Core 7 api.
- This solution use JWT for authentication of users and LTR for combined with ElasticSearch to give the best user experience.
- It contains 1 Web Api App and 2 Library apps
- The Web Api app calls the Common and Services App, while the Services App call only the Common App.
- The purpose of this distribution is to garanty the reusability of the services.
- The services can now be called in other solutions if needed.
- The Main Web Api application contains controllers, datas and configs necessary for running the app.
- The Common Library app contain the contexts, models (entities and data transfert objects), Enumerations, Exceptions and Interfaces.
- The Services Library app contain the providers and services used by the Web Api app.

# To Run the application:
- Adjust the ConnectionStrings, JwtConfiguration and ElasticsearchConfiguration in TestAssignment/appsettings.Development.json file.
- Go to the Root Directory of the Main app TestAssignment
- Run the command : dotnet ef migrations add migration1 --context InteractionDbContext
- Run the command : dotnet ef database update --context InteractionDbContext
- Run an elastic search server.
- Run the Server App.
- Run the angular App in the link: https://github.com/ouailmj/TestAssignmentFront.
