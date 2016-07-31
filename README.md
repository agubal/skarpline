# How to start project:
1) Change connection string in Skarpline.Api\Web.Config file. Or feel free to use current azure connection.
2) Run migration (if you've changed connection string in Step 1): Update-Database -ProjectName Skarpline.Data -StartUpProjectName Skarpline.Api
3) Restore nuget packages
4) Set 2 startup projects: Skarpline.Api and Skarpline.Chat
5) Start multiple instances of Skarpline.Chat project to emulate chat usage by users.