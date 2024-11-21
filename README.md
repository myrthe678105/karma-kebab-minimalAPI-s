# Mock API / karma-kebab-minimalAPI-s

## Login Flow

The implemented login flow is the **Resource Owner Password Credentials (ROPC) Grant**. This flow allows a user to obtain an access token by directly providing their username and password. The credentials are validated against hardcoded data in a JSON file (`db/login.json`), and upon successful authentication, an access token and other related information are returned.

## Run the project

`dotnet run`

## Access Swagger documentation

http://localhost:5096

## Explanation

1. Database - will be a .json file for each microservice (e.g., event.json, microservice.json, etc.)

2. Models - contains the objects needed for mapping the data

3. Services - where the entire logic will happen for CRUD operations

4. Program - configure Swagger and define routes