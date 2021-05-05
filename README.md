# Prerequisites

- .NET Core 3.1 Runtime
- Docker
- Visual Studio
- Visual Studio Code (best for viewing the client code in my opinion)
- Powershell
- Node >= 10.16 and npm >= 5.6 
- Ensure you have proper environment variables set up to run docker and powershell commands.

# Couchbase Configuration

Navigate to ShippingApi\ShippingApi and run the command `pwsh -f pre-build.ps1`.
This will start and configure the couchbase Docker container for the project.

If this does not work the manual configuration documentation can be found here: 
[Couchbase Docker Container Setup](https://docs.couchbase.com/tutorials/quick-start/quickstart-docker-image-manual-cb65.html)

Some potential issues can be resolved by restarting docker and purging data. (docker desktop -> troubleshoot -> clean/purge data)

Note: If you are curious, the Couchbase WebUI can be accessed at: http://localhost:8091. 
- username: Administrator 
- password: password. 

# Running the Api
- Open and build the solution in Visual Studio
- Select ShippingApi from the run dropdown and run the project

# Running the Api Tests
- The database must already be configured for this.
- These tests do require that you have already run the Api project which loads the required data.
- Open the project in Visual Studio and navigate to the test explorer, click run all. 

# Running the client
Navigate to the folder ShippingFrontend/shipping-app and run:

`npm i`

`npm start`
