## Pokemonsieur [![Pokemonsieur](https://circleci.com/gh/devdotcore/pokemonsieur.svg?style=svg)](https://app.circleci.com/pipelines/github/devdotcore/pokemonsieur?branch=master)

![Last Build](https://github.com/devdotcore/pokemonsieur/workflows/.NET%20Core/badge.svg?branch=develop)
![Docker Build](https://github.com/devdotcore/pokemonsieur/workflows/Docker%20Build/badge.svg)

The purpose of this API is to get the pokemon details from [Pokemon Api](https://pokeapi.co/) by passing the valid pokemon name and translate its description to Shakespearean using [Fun Translation Api](https://funtranslations.com/shakespeare).

**This API uses [Swagger](https://swagger.io/), so you can test its endpoint easily via browser**

### Try a pre-built version
The lastest image for Pokemonsieur API is available on [Docker Hub](https://hub.docker.com/). You can follow the steps below f and test the API without cloning this repository.

1. Install [Docker Desktop](https://www.docker.com/products/docker-desktop) on your machine.
2. Start docker desktop application, ensure its running properly.
3. Get the latest image from of [Pokemonsieur](https://hub.docker.com/repository/docker/kuldeepbhakuni/pokemonsieur) from Docker Hub by running the following command.
```markdown
docker pull kuldeepbhakuni/pokemonsieur:latest
```
4. Start the container i.e. -
```markdown
docker run --rm -it -p 8080:5000 kuldeepbhakuni/pokemonsieur:latest
```
5. API should be up and running, access it locally on https://localhost:8080

### Setup
The following steps are for runnning the codebase locally on a Mac using Visual Studio Code, you find similar or better options online.

1. Download and install [.Net Core 3.1.200](https://dotnet.microsoft.com/download/dotnet-core/3.1) SDK on your machine.
2. Dowload this repo into a working directory and take latest from master branch
```markdown
git clone git@github.com:devdotcore/pokemonsieur.git
```
3. Open the project in [VS Code](https://code.visualstudio.com/) and build -
```markdown
dotnet build
```
4. Make sure all the test case are running -
```markdown
dotnet test
```
5. Start the project directly on VS Code or run the following command -
```markdown
dotnet run --project ./src/Pokemonsieur.Shakespeare/Pokemonsieur.Shakespeare.csproj
```
5. By default, the API will be available on https://localhost:5001

### Docker Compose
If you have [Docker Desktop](https://www.docker.com/products/docker-desktop) installed and running on you machine, you can deploy the application locally using docker.

```markdown
 docker build -t [IMAGE_NAME]:[TAG] .  
```
once build is complete, you can start the container -

```markdown
docker run --rm -it -p [PORT_NUMBER]:5000 [IMAGE_NAME]:[TAG]
```

API should be available locally -

```markdown
https://localhost:[PORT_NUMBER]/
```