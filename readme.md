# Overview
This is a simple project to demonstrate the use of Managed Identity and Azure Open AI.

# Instructions
Create Azure Open AI Service to your resource group.
Deploy GPT-40 model.

Deploy infra using the following commands:
```bash
azd auth login
azd up
```
Copy sample.env to .env and put the AZURE_OPENAI_ENDPOINT: https://<your_name>.openai.azure.com

Run the project using the following commands:

```
dotnet workload restore --project ./Project.csproj
dotnet run --project ./Project.csproj
```