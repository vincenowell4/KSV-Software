# Requirements Workup

# Feature 3: View results
## Elicitation
* The goal is well defined and we understand that the goal of this feature is to view the results of a created vote and allow for users that are logged in, and were logged in when they created the vote, to view created vote results and get analytics on the results of the vote in forms of graphs and stats. 
* How is anonymous data being stored and viewed?
* How am I going to save the data?
* Can I send the data results to other people if I wanted to?
* The scope is clear and we understand that everything relating to viewing results, this includes viewing statistics and getting an analysis of the data is included. 
* In what format are locally saved results going to be stored?

## Analysis
* Anonymous data storage requires more elicitation from our client. This may be a feature that is implemented later as we further into the development process and understand what needs to be done to achieve this goal. 
* Data will be stored on the database accesible to the user account that created the vote. We also hope to implement features to allow users to download results and statistics of the vote on their machine via CSV.
* You may download the results of the vote or send them a shareable link with the results. 
* We are hoping to have the results downloadable as a CSV. We also  hope to  have the results viewable in the app. 
* The only conflict we may run into is how to handle anonymous votes within the database in such a way that we can verify the identity of someone to eliminate the double votes but maintain confidentiality within the vote. 


## Design and Modeling

<br/>
<img src="VotingAppClassDiagarm.drawio.svg">

## Analysis of the Design
* The data model allows for votes to be viewed after being created and voted on. It allows to a user to get the data from the created vote model, and gather the sumbitted votes to allow for evaluation and get the stats of the vote. it will also allow for later parsing of the data into csv format for download.
* For the given moment we believe that the data model does meet all requirements for the feature, it is the case that changes to the data model will be made as we further along in the development process and we discover more item that are needs to meet these requirements.
