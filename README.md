# Bluefoxglove - Server
## Description

This repository contains the BlueFoxglove API project, a game that demonstrates how to implement real-time communication using SignalR. It also includes unit tests using the NUnit framework with NSubstitute for mocking.

## Getting Started
Prerequisites
Before you begin, ensure you have the following prerequisites installed:

.NET 6 SDK or later
Visual Studio or Visual Studio Code (optional)
NUnit and NUnit Test Adapter (for running tests in Visual Studio)
Nsubsitute (for mocking)
MongoDB.Driver (for persistence)


## Installation 
git clone https://git.ardentheartgames.com/neutral-good-mentorship-public/blue-foxglove/blue-foxglove-server.git

### Build Project
dotnet build

### Run Project
dotnet run

## Project Structure
The project structure is organized as follows:

Controllers: Contains API controllers for handling HTTP requests.
Hubs: Contains SignalR hub class for real-time communication.
Tests: Contains unit tests written using NUnit and NSubstitute

## Usage
Navigate to https://localhost:7130/swagger/index.html to access the Swagger documentation and explore the available API endpoints.

## Testing
dotnet test
