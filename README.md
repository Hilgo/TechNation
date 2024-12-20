# TechNation API

TechNation API is a web application that provides logging services. It allows users to convert logs from Minha CDN format to Agora format, save logs to a database, and retrieve logs from the database or a file.

## Table of Contents

- [TechNation API](#technation-api)
  - [Table of Contents](#table-of-contents)
  - [Features](#features)
  - [Technologies](#technologies)
  - [Getting Started](#getting-started)
    - [Prerequisites](#prerequisites)
    - [Installation](#installation)
    - [Configuration](#configuration)
  - [Usage](#usage)
    - [API Endpoints](#api-endpoints)
  - [Running Tests](#running-tests)
  - [Contributing](#contributing)
  - [License](#license)
  - [Contact](#contact)

## Features

- Convert logs from Minha CDN format to Agora format.
- Save logs to a database.
- Retrieve logs from the database or a file.
- Asynchronous logging to avoid blocking the main thread.
- Comprehensive error handling and logging.

## Technologies

- .NET Core 2.0
- C# 7.3
- ASP.NET Core
- Entity Framework Core
- AutoMapper
- Moq
- NUnit

## Getting Started

### Prerequisites

- .NET Core SDK 2.0 or later
- SQL Server or any other supported database
- Visual Studio 2022 or any other compatible IDE

### Installation

1. Clone the repository:
   **git clone https://github.com/Hilgo/TechNation.git cd TechNationAPI**

2. Restore the dependencies:
   **dotnet restore**
   
3. Build the project:
   **dotnet build**
   
### Configuration

1. Update the `appsettings.json` file with your database connection string and file logging settings:
   `{ "ConnectionStrings": { "DefaultConnection": "YourDatabaseConnectionString" }, "FileLogging": { "LogDirectory": "Logs", "LogFileName": "log.txt" } }`
   
2. Apply the database migrations:
   `dotnet ef database update`
   
## Usage

### API Endpoints

- **Convert Log**: Convert a log from Minha CDN format to Agora format.
  - **POST** `/api/logs/convert`
  - Request Body:
    `{
      "MinhaCdnLog": "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2",
      "FormatoSaida": 0
    }`
    
- **Convert Log from ID**: Convert a log from Minha CDN format to Agora format using the log ID.
  - **POST** `/api/logs/convertFromId`
  - Request Body:
    `{
      "Id": 1,
      "FormatoSaida": 0
    }`
    
- **Convert Log from Query String**: Convert a log from Minha CDN format to Agora format using a query string.
  - **GET** `/api/logs/convert?minhaCdnLog=312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2`

- **Save Log**: Save a log to the database.
  - **POST** `/api/logs/SaveLogAsync`
  - Request Body:
    `{
      "MinhaCdnLog": "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2"
    }`
    
- **Get Log by ID**: Retrieve a log from the database by ID.
  - **GET** `/api/logs/{id}`

- **Get All Logs from Database**: Retrieve all logs from the database.
  - **GET** `/api/logs/GetAllLogsDataBaseAsync`

- **Get All Logs from File**: Retrieve all logs from a file.
  - **GET** `/api/logs/GetAllLogsFile`

## Running Tests

1. Navigate to the test project directory:
   `cd TechNationTest`
   
2. Run the tests:
   `dotnet test`
   
## Contributing

Contributions are welcome! Please open an issue or submit a pull request for any improvements or bug fixes.


## Contact

For any questions or inquiries, please contact [lucas.stabile93@gmail.com](mailto:lucas.stabile93@gmail.com).
