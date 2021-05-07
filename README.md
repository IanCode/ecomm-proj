# Ecommerce Shipping

## Prerequisites

- .NET Core 3.1 Runtime
- Docker
- Visual Studio
- Visual Studio Code (best for viewing the client code in my opinion)
- Powershell
- Node >= 10.16 and npm >= 5.6 
- Ensure you have proper environment variables set up to run docker and powershell commands.

## Couchbase Configuration

In command prompt navigate to ShippingApi/ShippingApi and run the command `pwsh -f pre-build.ps1`. I wanted to include this as a pre build action but there were a few issues. This will start and configure the couchbase Docker container for the project.

If this does not work the manual configuration documentation can be found here: 
[Couchbase Docker Container Setup](https://docs.couchbase.com/tutorials/quick-start/quickstart-docker-image-manual-cb65.html).

Some potential issues can be resolved by restarting docker and purging data. (docker desktop -> troubleshoot -> clean/purge data).

Note: If you are curious, the Couchbase WebUI can be accessed at: http://localhost:8091. 
- username: Administrator
- password: password

## Running the Api
Open and build the solution in Visual Studio.

Select ShippingApi from the run dropdown and run the project.

## Running the Api Tests
The database must already be configured for this.

Open the project in Visual Studio and navigate to the test explorer, click run all. 

## Running the client
Navigate to the folder ShippingFrontend/shipping-app and run:

`npm i`

`npm start`

## Additional Notes/Potential Features

I know npm has a calendar control that could be added to the product card. Would require another API endpoint for updating the product's order date and resulting ship date. 

Integrating the spinner for ui loading functionality.

Add manufacturers and holidays.

Prettify the UI, separate scss files and utilize global colors, font sizes, etc. in design system.

Add more detailed instructions on environment setup.

More unit tests.