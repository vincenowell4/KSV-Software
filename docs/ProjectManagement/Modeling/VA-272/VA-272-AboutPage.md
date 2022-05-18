# VA-272


## Title

*As a user I would like to see an About page so that I can get more information about the app and who created it*


## Description

This user story is for adding an About page that contains information about the Opiniony voting app. It will include:

--A description of the reason for the project, dates and place: Western Oregon University, CS 460, CS 461, and CS 462 Software Engineering I, II and III, Senior Capstone course, etc.
--All team members names with links to your Git repos and optionally LinkedIn
--A link to your project's Git repository


### Details:

1. Add a new link in the nav bar called 'About'
2. Create a new endpoint in the HelpController for the About page.
3. Add the About page, with all the information on it as specified above, in the description.

See the UI mockup in Modeling and Other Documents, below.


## Acceptance Criteria
As a user, if I navigate to the About page, I will see information about the Opiniony voting app and the team that created it


## Assumptions/Preconditions
None


## Dependencies
None


## Effort Points
1


## Owner
Vince Nowell


## Git Feature Branch
f_about-page


## Modeling and Other Documents

Mockup and class diagram: 

![About Page Mockup](https://github.com/vincenowell4/KSV-Software/blob/f_about-page/docs/ProjectManagement/Modeling/VA-272/VA-272-UI-Mockup.png)


## Tasks
1. Create a new page, About.cshtml, with the required content
2. Add a new item to the navbar in _Layout.cshtml for the About page
3. Add a new endpoint to the Help Controller for the About page
