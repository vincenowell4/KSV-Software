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
Our first goal is to create a **data model** that will support the initial requirements.

1. Identify all entities;  for each entity, label its attributes; include concrete types
2. Identify relationships between entities.  Write them out in English descriptions.
3. Draw these entities and relationships in an _informal_ Entity-Relation Diagram.
4. If you have questions about something, return to elicitation and analysis before returning here.

## Analysis of the Design
The next step is to determine how well this design meets the requirements _and_ fits into the existing system.

1. Does it support all requirements/features/behaviors?
    * For each requirement, go through the steps to fulfill it.  Can it be done?  Correctly?  Easily?
2. Does it meet all non-functional requirements?
    * May need to look up specifications of systems, components, etc. to evaluate this.

