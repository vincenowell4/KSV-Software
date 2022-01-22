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

