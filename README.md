# AI Playground — Backend API

*A backend for experimenting with and benchmarking prompts across multiple LLM providers.*

![.NET 8](https://img.shields.io/badge/.NET%208-512BD4?logo=dotnet&logoColor=white)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-Web%20API-512BD4?logo=dotnet&logoColor=white)
![EF Core](https://img.shields.io/badge/EF%20Core-SQL%20Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?logo=swagger&logoColor=black)

AI Playground is a tool for **prompt engineering and provider comparison**. You organise prompts
into scopes, run them against models from several AI providers, and the API **automatically scores
each response** against the answer you expected — so you can compare providers and settings
objectively instead of by gut feeling.

This repository is the REST API. The React front end lives in
**[AIPlaygroundFE](https://github.com/arobertt/AIPlaygroundFE)**.

## How it works

The domain is built around three concepts:

| Concept | Description |
|---------|-------------|
| **Scope** | A category that groups related prompts (e.g. *Summarisation*, *Code*). |
| **Prompt** | A named test case: a system message, a user message, and the *expected result*. |
| **Run** | The execution of a prompt against a chosen model at a given temperature. Stores the model's actual response plus two scores. |

When a run is created, the matching provider is called to generate a response. A second LLM call
then **grades that response against the expected result on a 1–100 scale**, and the score is stored
alongside the response. Users can also add their own rating, so automatic and human judgement sit
side by side.

## Supported providers

| Provider | Implementation |
|----------|----------------|
| OpenAI | `OpenAIProcessor` (official `OpenAI` SDK) |
| DeepSeek | `DeepSeekProcessor` |
| Google Gemini | `GeminiProcessor` |

Adding a provider means implementing `IAIProcessor`, registering it in `AIProcessorFactory`, and
adding a value to the `PlatformType` enum — the rest of the pipeline is unchanged.

## Architecture

A clean three-layer solution:

```
AiPlayground                 → ASP.NET Core Web API (controllers, DI, Swagger, CORS)
AIPlayground.BusinessLogic   → services, DTOs, and the AI processing layer (factory + processors)
AiPlayground.DataAccess      → EF Core DbContext, entities, configurations, migrations, repositories
```

The business logic depends on abstractions (`IRepository<T>`, `IAIProcessor`), the AI providers are
resolved through a factory, and data access uses the repository pattern — making the system easy to
test and extend.

## Getting started

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB / Express is fine)
- API keys for whichever providers you want to use

### 1. Configure API keys

Keys are read from environment variables (never committed to source):

```bash
setx OPENAI_API_KEY   "your-key"
setx DEEPSEEK_API_KEY "your-key"
setx GEMINI_API_KEY   "your-key"
```

### 2. Configure the database

Update the `AIPlaygroundContext` connection string in `AiPlayground/appsettings.json` to point at
your SQL Server instance, then apply the migrations:

```bash
dotnet ef database update --project AiPlayground.DataAccess --startup-project AiPlayground
```

### 3. Run

```bash
dotnet run --project AiPlayground
```

The API starts on `https://localhost:7004`, with interactive Swagger docs at
`https://localhost:7004/swagger`.

## Project structure

```
AiPlayground.sln
├── AiPlayground/                   # API layer — controllers, Program.cs, appsettings
├── AIPlayground.BusinessLogic/     # Services, DTOs, and AIProcessing (factories + processors)
└── AiPlayground.DataAccess/        # DbContext, entities, EF configurations, migrations, repositories
```
