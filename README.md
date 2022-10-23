<a name="readme-top"></a>

<!-- PROJECT LOGO -->
<div align="center">
  <h3 align="center">Calculation of payments per employee .Net Core :rocket:</h3>
  <p align="center">
    This is the proposed solution for calculating payments per employee of the ACME company.
    <br />
  </p>
</div>

<!-- TABLE OF CONTENTS -->
<details>
  <summary>Table of Contents</summary>
  <ol>
    <li>
      <a href="#about-the-project">About The Project</a>
      <ul>
        <li><a href="#how-does-the-api-work">How does the API work?</a></li>
      </ul>      
    </li>
    <li>
      <a href="#getting-started">Getting Started</a>
      <ul>
        <li><a href="#installation">Installation</a></li>
        <li><a href="#running-unit-tests">Running unit tests</a></li>
      </ul>
    </li>
    <li><a href="#usage">Usage</a></li>
  </ol>
</details>

<!-- ABOUT THE PROJECT -->
## About The Project

This is the proposed solution for the exercise of the payment of hours by the employees of the ACME company. At the level of framework and unit test components, it is built with .Net Core 6, NUnit, and Mock using a Microservices architecture. This type of architecture is chosen anticipating the possible use of the service through different clients (web, desktop).

The solution is structured following a clean architecture scheme. There you can find several solution folders and subfolders namely:

![image](https://user-images.githubusercontent.com/59126066/197370120-c023108e-859d-4f3a-beae-12a49fd7d76d.png)

:file_folder: Files folder

![image](https://user-images.githubusercontent.com/59126066/197370138-a0d03b36-4b48-4178-9639-db80ec2552cf.png)

In this folder you can find two subfolders:

* **Configuration:** here you can find the file _ReferenceValuesByTimeRange.json_ which contains the settings for time ranges and pay-per-day values. This file is used by the service as a reference matrix to calculate payments.
* **Samples:** here you can find an example file with data and time ranges for different employees.

:file_folder: src folder

![image](https://user-images.githubusercontent.com/59126066/197370423-d47051a3-3823-4810-ae31-54d0b5ccc746.png)

In this folder you can find two subfolders:

* **API:** here you can find an Asp.Net Core Web API project with an endpoint to upload the payments file. At the level of the "appsettings.json" file there are several settings that are used to analyze and parse the content of the file.

![image](https://user-images.githubusercontent.com/59126066/197370545-0ba7d088-cb68-43a8-9e24-227a686dabb9.png)

* **Core:** here you can find two libraries. The _ACME.PayrollManagement.Application_ library contains all the logic related to saving the file in a local copy on the server, parsing the file content, and calculating payments. The _ACME.PayrollManagement.Domain_ library contains classes that are transversal to the solution such as DTOs and enumerations.

![image](https://user-images.githubusercontent.com/59126066/197370673-d1ad3081-22aa-4943-9333-59985f91abd3.png)

:file_folder: test folder

Here you can find two unit test projects. The first is focused on unit tests of API controllers. The second contains unit tests for the methods of saving, parsing, and calculating payments.

![image](https://user-images.githubusercontent.com/59126066/197370841-cf97a246-70a8-4a56-bcbe-8822685bb11f.png)

## How does the API work?

When uploading the file using the API, the following steps are executed:

1. Validates if the file extension is valid.
2. It is validated that the file has content.
3. If these validations are successful, a copy of the file is saved to a local path.
4. The content of the file is parsed using the separators. Here it is validated if the names of the employees are correct, as well as the day identifiers and time ranges.
5. To process payments, it is necessary that the content of the file can be parsed in its entirety. Otherwise, the process stops, and an error message is generated with the line containing the error.
6. If the previous step is completed, the payments per employee are calculated and a list is returned with the names and amounts to be paid for each employee. The currency used is also displayed.

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- GETTING STARTED -->
## Getting Started

To run the solution you must have the latest version of .Net Core installed ([.NET Core SDK](https://www.microsoft.com/net/download)). This way you will be able to execute the commands to compile and run the solution.

```console
dotnet build
dotnet run
```

### Installation

_Follow these steps to run the solution locally:_

1. Clone the repo
   ```sh
   git clone https://github.com/Jos3Rodrigu3zL3arning/Demos.git
   ```
2. Open the solution (.sln file) from Visual Studio 2022.
3. Restore the NuGet packages for the solution from this option:

![image](https://user-images.githubusercontent.com/59126066/197371738-c792a0df-897e-4d16-998c-69b617b2c8e3.png)

4. Compile the entire solution.

![image](https://user-images.githubusercontent.com/59126066/197371820-1f32e717-b9e7-4117-88db-eaf2f58c9852.png)

5. Before running the solution, verify that the **ACME.PayrollManagement.API** project is selected as the startup project. If it isn't, right-click on it and select the **Set a Startup Project** option.

![image](https://user-images.githubusercontent.com/59126066/197371914-dbe3de9e-b378-40c9-80a5-d928bf2fcde1.png)

![image](https://user-images.githubusercontent.com/59126066/197371931-a8980205-aaf4-4887-a7b9-e0d9fbe41b25.png)

6. Lastly, make sure that in the API configuration files (_appsettings.json_ and _appsettings.Development.json_) the **PathFilesToProcess** key has a path that exists on your local machine.

![image](https://user-images.githubusercontent.com/59126066/197372085-b299139d-0d2d-42a6-b25e-5a59ffc4b01b.png)

### Running unit tests

_Follow these steps to run the unit test for the solution:_

1. In Visual Studio 2022, open the **Text Explorer** panel:

![image](https://user-images.githubusercontent.com/59126066/197372766-49790d5e-6f60-4e0a-a2a5-63e807816e40.png)

2. Run the unit tests as required.

![image](https://user-images.githubusercontent.com/59126066/197372795-d72c812f-559a-47c0-961b-42cc4620aa36.png)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

<!-- USAGE EXAMPLES -->
## Usage

Running the API opens a browser instance with a Swagger interface. There, an available endpoint is shown to load the .txt file to be processed.

![image](https://user-images.githubusercontent.com/59126066/197372302-97e3b287-e122-4ad4-b5b7-5901e33e3193.png)

Click on the endpoint and then on the **Try it out** button. Select the file and click the **Execute** button.

![image](https://user-images.githubusercontent.com/59126066/197372505-a59cec00-bf10-4aa3-b449-58d1bb8e6927.png)

![image](https://user-images.githubusercontent.com/59126066/197372580-315bde66-6e3f-42fc-b910-70c33c09822b.png)

When executing the endpoint, the result is generated with the calculation of the payments.

![image](https://user-images.githubusercontent.com/59126066/197372648-359bc809-a49f-4a4f-a8b2-50e3e17f5e24.png)

If the file has any inconsistencies, the endpoint generates an error message.

![image](https://user-images.githubusercontent.com/59126066/197373395-7b6d6949-68f5-44b1-bced-7e1ad9e176b6.png)


<p align="right">(<a href="#readme-top">back to top</a>)</p>
