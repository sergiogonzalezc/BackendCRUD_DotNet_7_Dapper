# Backend project configuration

## Database Configuration ##
You must execute the scripts that are in the **BackendCRUD.Sql** folder in the Solution. This files apply to SQL Server 2016 or higher (Express, Standard, Enterprise, etc.), in the order indicated.
- **Note:** Script 1 creates a DB from scratch in the location **C:\MSSQL_BD** which must have been previously created. If you are not running SQL Server locally, you can skip this step and move on to script 2 and script 3.
- **Script 2**: If you don't have a DB with **BD_Team** name, please create a new DB with that name. Then execute **Script 2** for create only the tables.
- **Script 3**: Create only example records for members, role types and tags for members. The **Script 2** must be executed previous to execute **Script 3**.

## Open project in Visual Studio ##
To run the backend in **DEBUG** mode, you must have installed:
- [.Net 7x SDK]
- [VS 2022 Community Edition]

## Connection string configuration:##
You must go to the **appsettings.json** file and you must edit the **connectionstring**, you will see the values with **XXXXXX**.
-Server Host
- Server Port
- User
- Password

"connectionstring": "Server=**XXXXXX**,**XXXXXX**;Initial Catalog=**BD_Team**;User ID=**XXXXXX**;Password=**XXXXXX**;TrustServerCertificate=true"

To start debugging, open **sln** solution and run F5:

**Important:**
Project type: Api .Net core

Main dependencies:
- [Entity Framework]
- [MediatR]
- [Automapper]
- [Swashbuckle.AspNetCore]
- [NLOG]


## Supported project to create Docker image. Steps to create image:##
- [1] Start "Docker Desktop for Windows" v4.25.

- [2] Open Windows, type WINDOWS + R key shortcut, and open **Developer Powershell Visual Studio Community Edition 2022** as Administrator, and then execute:

Note: First you must must go to the path where the **.sln** file is, for example: cd "E:\Backend_Net_CRUD_salary\"

**docker image build -t crudchallenge:1.0 -f .\BackendCRUD.Api\Dockerfile .**

- [3] Open "Docker for Desktop Windows", go to the image list and get the **"GUID"** of the newly created new image,
  
- [4] Create container from the previously created image, go to **Developer Powershell Visual Studio** and execute:

   **docker container create BackendCRUD-container -p 18001:**5000** GUID**

    Note: the port 5000 is the internal PORT configured in the .dockerfile in the API proyect of the solution.
    Note: the port 18001 is the external PORT of the container and it contect with the port 5000.

- [5] Start the container:
   **docker container start BackendCRUD-container**

- [6] The container will be started on port **18001**. To validate: open the browser at **http://localhost:18001/swagger/index.html** you should see the list of exposed methods to verify using **swagger**. You may change the **localhost** server for the location of your container. 

# Basic instructions and use cases
1. Insert a rol type: You can insert a new Role Type 
Endpoint: api/RoleType/InsertRoleType

2. Optionally, you can Insert a new Tag for a existing member. If the tag does not exist, it will be created automatically before assigning to the member
Endpoint: api/Tag/InsertMemberTag

3. Insert a new Employee Type member. You must write the Role Type (1,2,3,4 etc), and Country. The name must be unique.
Endpoint: api/Tag/InsertMember

4. Insert a new Contractor Type member.
Endpoint: api/Tag/InsertMember


Note: if the type is "E" (employee), the API get the currencie in Country based, over the next external API: https://restcountries.com/v3.1/name/chile

    Note: The API validate min, max, and amounts of different input fields..
    Note: The API doesn't implement JWT Secure Token because was thinking in directly test backend in swagger or Postman.
