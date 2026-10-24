# See Sharp: learning modern C#

See Sharp is a project where I learn modern C#. It's meant to simulate the software used at an optometrist's office. The goal is to have things like:

- A backend API
- A database (in this case, SQLite)
- OpenAI integration
- Redis 
- Web UI frontend with Blazor
- Docker Compose for containerization
- Accessing a 3rd party API in the backend
- Logging
- Websockets using SignalIR
- Unit testing
- OpenAPI documentation using Swagger

# Running the project at the command line

From the project root:

- Backend: `dotnet run --project src/SeeSharp.Api/SeeSharp.Api.csproj`
- Frontend: `dotnet run --project src/SeeSharp.Client/Seesharp.Client.csproj`

# AI usage

I used AI to learn faster, such as:

- Letting it write the repetitive code in the API endpoints
- Asking questions beyond what a tutorial would typically cover ("how do I put the API endpoints in different files?")