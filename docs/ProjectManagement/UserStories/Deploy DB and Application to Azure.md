# Deploy DB and Application to Azure
<hr>

## Title

*As a user I want to go to the Voting App web site and see in the address bar that the application ends with azurewebsites.net, meaning it is deployed in the Azure cloud.*

## Description

This user story is about deploying both the database and the Voting App web application to Azure. Once deployed, a user can navigate to the web application using a URL which will end with azurewebsites.net, and that will indicate to a user that the web application has been deployed to Azure. 

### Details:

1. Browser address bar for the web application ends with azurewebsites.net

## Acceptance Criteria
No .feature file for this one

    Given I am on the "Home" page 
    Then I will see that the address bar contains a URL that ends with azurewebsites.net 


## Assumptions/Preconditions
Initial project has been set up with the database


## Dependencies
Create blank project


## Effort Points
1 to 2


## Owner
Vince Nowell


## Git Feature Branch
f_deploy_db_and_app_to_azure


## Modeling and Other Documents
Not applicable


## Tasks
### Deploying the database

NOTE: This process is documented in "How to Deploy Databases and Web Apps to the Cloud Using Azure.rtf"
1. Create SQL Server Instance
2. Create application database in Azure
3. Deploy web application to Azure
4. Set up Identity authorization db on Azure


###
