#Project Variables
PROJECT_NAME ?= SunnyCoffee
ORG_NAME ?= SunnyCoffee
REPO_NAME ?= SunnyCoffee

.PHONY: migrations db

migrations: 
		cd ./SunnyCoffee.Data && dotnet ef --startup-project ../SunnyCoffee.Web/ migrations add $(mname) && cd ..
db: 
		cd ./SunnyCoffee.Data && dotnet ef --startup-project ../SunnyCoffee.Web/ database update && cd ..