# MongoDbConnector

Small C# class library that wraps a MongoDB connection and exposes a PingAsync() method.

## Prereqs
- .NET 9 SDK
- Docker Desktop running (for Testcontainers-based tests)

## Build & Test

The tests spin up a temporary MongoDB in Docker via Testcontainers and validate both success and failure cases of PingAsync().

## Project Layout
- MongoDbConnector/ – library (MongoDBConnector class)
- MongoDbConnector.Tests/ – xUnit tests using Testcontainers
