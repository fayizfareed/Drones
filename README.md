Drones-v1
---------------------
In addition to the existing description provided in Drones-v1.pdf, I have included a new property called "Id" in both the Drone and Medication objects. This serves as a unique identifier that can be used as a primary key for these objects and as a foreign key for other tables or objects. By using this identifier, we can easily distinguish between different instances of these objects.

Furthermore, I have added a new property called "State" to the Medication object. This is because after a drone has been loaded and delivered to a customer, it may return to an idle state or a loading state to pick up additional items. In such cases, the previously used medication may still be in the drone. To avoid using the same medication again and to keep track of the currently loading item, I have introduced the "State" property.

In addition to the previous changes, I have also made the following assumptions while developing this code:
	01. I have assumed that the picture of the medication might be either a base64 image or an image URL. Therefore, I have made the "image" property of the Medication object as a string.
	02. I have assumed that the serial number of each Drone is a unique property, as each drone will have a distinct serial number assigned to it.
	03. I have also assumed that the medication code is a unique property, as each medication will have a distinct code assigned to it.


Solution Structure
---------------------
This solution consists of two projects. The main project is called "Drones_WebAPI", which serves as the startup project, and the second project is called "TestDronesWebAPI", which includes all the xUnit tests for the main project.
01. Drones_WebAPI
	* The "Properties" folder contains a single "launchSettings.json" file, which contains all the necessary settings to launch the application.
	* The "Controllers" folder contains a single controller that handles all the logic and serves the endpoints for the application.
	* The "DataAccess" folder contains a custom database context file which consists of Entity Framework Core database configurations for the application.
	* The "DTO" folder consists of Data Transfer Objects, which are passed as parameters to the request as well as returned objects from endpoints in the application.
	* The "Global" folder consists of some files that contain all the global values and methods used throughout the application.
		- The "Enum" files contain necessary enumerations like drone models, drone state, and medication state that are used throughout the application.
		- The "Global" folder contains database seeders, which are used to populate the database with initial data.
		- The "MyEventLog" contains all the logging configuration and methods used in the application.
		- The "PeriodicTask" folder contains the battery level checking logic and background work thread, which are used to periodically check the battery level of the drones.
	* The "Model" folder contains all the necessary models needed for the project.
02. TestDronesWebAPI
	This project contains three classes:
	* "DroneControllerTest" class, which contains all the unit test cases for the project.
	* "Helper" class, which includes all the assert methods used in the test cases.
	* "Usings" class, which contains global using namespaces used in the project.


Development
---------------------
I have used .NET Core to ensure cross-platform compatibility of the application, and I have also utilized .NET Framework 6.0 as it is the latest stable version available. Additionally, I have employed Entity Framework Core to utilize an in-memory database for the application.


Build
---------------------
This project has already been built and is ready to use. The build location is located at "\Drones_WebAPI\bin\Debug\net6.0". If you need to rebuild the project, you can open the solution in Visual Studio 2022, right-click on the "Drones_WebAPI" project, and select "Build". The project will be built without any errors.

Run
---------------------
To run the application, simply double-click on the "\Drones_WebAPI\bin\Debug\net6.0\Drones_WebAPI.exe" file. This will launch the application and serve the endpoints using the Kestrel server. Once the application is open, you can see the terminal running Kestrel and the app will automatically open a Swagger test page in your default browser.

The logging section is used to log the battery level. To enable the logging section, you must run the "\Drones_WebAPI\bin\Debug\net6.0\Drones_WebAPI.exe" file as an administrator. To do this, right-click on the "\Drones_WebAPI\bin\Debug\net6.0\Drones_WebAPI.exe" file and select "Run as administrator". Note that if you simply double-click to run the app, the logging section will not work.

The following endpoints are available in this project:
	* /api/Drone/RegisterDrone							- POST | Register a new drone
	* /api/Drone/LoadDrone								- POST | Load medication into a drone
	* /api/Drone/ChangeDroneState/{id:long}				- PUT  | Change a drone's state to something other than its current state (e.g., Delivered, Loading, Idle) by specifying the drone's ID
	* /api/Drone/ChangeBatteryLevel/{id:long}			- PUT  | Manually change a drone's battery level by specifying the drone's ID
	* /api/Drone/GetDronesBySerilNumber/{serialnumber}	- GET  | Retrieve information about a drone by its serial number
	* /api/Drone/GetDroneById/{id:long}					- GET  | Retrieve information about a drone by its ID
	* /api/Drone/GetDrones								- GET  | Retrieve information about all drones
	* /api/Drone/GetLoadings/{id:long}					- GET  | Retrieve information about the medications currently loaded in a drone by specifying the drone's ID
	* /api/Drone/GetLoadingHistory/{id:long}			- GET  | Retrieve information about all loadings, including those that have been loaded and delivered, by specifying the drone's ID
	* /api/Drone/AvailableDrones						- GET  | Retrieve information about drones that are available for loading
	* /api/Drone/GetBatteryLevel/{id:long}				- GET  | Retrieve a drone's battery level by specifying the drone's ID


Test
---------------------
I have used xUnit to test all the endpoints, and the test cases are written in the DroneControllerTest.cs file. All the tests have already passed during development, but you can test them again using these test cases. To run the test cases, you need to open the solution in Visual Studio 2022 and either run the application without debugging or run the application directly from the build directory, as described in the "run" section. Then, go to the "View" menu in Visual Studio and select "Test Explorer." In the Test Explorer window, select "TestDronesWebAPI" and click the "Run" button.

Additionally, I have used Swagger to test all the endpoints in a graphical user interface (GUI). You do not need to use any third-party applications like Postman to test the endpoints. In the Swagger GUI, you can provide relevant data as parameters to the endpoint and test them.