# Overview
This is a simple project to demonstrate the use of Managed Identity and Azure Open AI.

# Instructions
Create Azure Open AI Service to your resource group.
Deploy GPT-40 model.
Copy sample.env to .env and put the AZURE_OPENAI_ENDPOINT: https://<your_name>.openai.azure.com
dotnet workload restore --project ./Project.csproj
dotnet run --project ./Project.csproj