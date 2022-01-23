# Requirements Workup

# Feature 1: Create a vote
## Elicitation

1. Is the goal or outcome well defined?  Does it make sense? <br/>The outcome of the end of the process is a vote with a description and voting choices in a room stored in the database that is acccesible through a room code for others to access. When creating a vote you will have the option to choose the voting type. 
2. What is not clear from the given description? <br/>How are we sharing the code? What types of votes will be available and will they be explained? 
3. How about scope? Is it clear what is included and what isn't? <br/>The scope is well defined. Scope of feature includes anything in relation to a new vote, this includes adding new votes to the database and creating the access code. 
4. What do you not understand?
    * How vote creation data is stored
    * Differences between being logged in for creating a vote vs. not being logged in for creating a vote 
5. Is there something missing? <br/> Are there accesiblity options for creating votes as well?

## Analysis   
* Items such as code sharing and types of votes will be analyzed later in the process: we hope to support multiple kinds of voting types and allow for multiple kinds of sharing options for the room code. 
* Vote data will be stored in the database in such a way where it will be parsed to and from a serialized data format in such a way that it can be displayed to the user easily. 
* When creating a vote as a logged in user it will store said vote in the database logged underneath that users ID, such that you can view created votes on your account and make changes to them.
* As far as accessiblity options when creating a vote, we hope to include a feature to allow vote descriptions to be added through speech to text, but this may be a feature later added in development. 
* From what we can tell at the moment, there should be no conflicts in requirements specified from the stakeholders for this specific feature. 


## Design and Modeling

<br/>
<img src="VotingAppClassDiagarm.drawio.svg">

* This data model, specifically the CreatedVote class, will allow the user to create a new vote give it a discription, specify if the vote was anonymous or not, allow for multiple options and get a list of casted votes.
* There are also methods in place to allow for modification of the class to allow for editing of the vote after creation and to allow for getting data on the vote after it has finished.

## Analysis of the Design

* For the given moment we believe that the data model does meet all requirements for the feature, it is the case that changes to the data model will be made as we further along in the development process and we discover more item that are needs to meet these requirements.

