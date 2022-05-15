# VA-269


## Title

*As an admin I would like more detail in the application's logs to help with troubleshooting and debugging of the application*


## Description

This user story is about adding two new fields to the AppLogs table: Class and Method. These will contain the current class file and
current method name for the event that is being logged, making it easier, when viewing the log, to see where in the code the event was logged.

We will also add the two columns to Admin Logging table on the Admin page, so these fields can be viewed by an administrator. 

And we will add code wherever an event is being logged, so that the current class and method will be tracked with the logging of the event.


### Details:

1. The Admin Logging table on the Admin Page will have a two new fields, Class and Method.
2. The AppLog table in the database will have these same two fields added to it.
3. The code to log an event (either Info or Error) will need to be modified to pass in the current class and method name to the code that logs an event.
4. Reflection will be used to get the class and method names, so class names and methods don't have to be hard-coded.

See the UI mockup in Modeling and Other Documents, below.


## Acceptance Criteria
As a user who is logged in as an administrator, if I navigate to the Admin page and I click on the 'Click here for logging information' button,
Then I will see the Admin Logging table with the fields Date, LogLevel, Class, Method and Description


## Assumptions/Preconditions
There is at least one user who is registered as an administrator in the database


## Dependencies
None - all necessary dependencies have already been completed


## Effort Points
2


## Owner
Vince Nowell


## Git Feature Branch
f_va-269-enhanced-logging


## Modeling and Other Documents

Mockup and class diagram: 

![Multi Round Vote Mockup](https://github.com/vincenowell4/KSV-Software/blob/f_va-269-enhanced-logging/docs/ProjectManagement/Modeling/VA-269/VA-269-UI-Mockup.png)
![Class diagram](https://github.com/vincenowell4/KSV-Software/blob/f_va-269-enhanced-logging/docs/ProjectManagement/Modeling/VA-269/VotingAppClassDiagram.drawio.svg)


## Tasks
1. Create two new string fields in AppLogs table: ClassNmae and MethodName
2. Modify the Admin page to add the two new fields to the Admin Logging table
3. Modify each instance where there is a logging event in code, to also pass in the class and method name, using reflection
