# Confirm Registered User By Email


## Title

*As a user I want to confirm that I am a registered user of the Voting App web site by clicking the link sent by the app to my email address.*

## Description

This user story is about confirming the registration of a user in the web site by email. If the user clicks the link sent by the app to their email address, it should take them back to the Voting App web site, mark in the database that they are a registered user, and display a page that indicates that the user has confirmed their registration in the Voting App. 

### Details:

1. The user will click the link in the email sent by the Voting App to the email address the user entered on the Registration page.
2. The link will take the user back to the Voting App, save their registration status in the database, and display a page that indicates that the user has confirmed their registration in the Voting App. 

## Acceptance Criteria
No .feature file for this one

    Given I have received a confirmation email from the Voting App
    Then if I click on the link in the email, I should be taken back to the Voting App web site, which should display a  page confirming that I am a registered user of the Voting App


## Assumptions/Preconditions
Initial project has been set up with the database


## Dependencies
Create blank project


## Effort Points
1


## Owner



## Git Feature Branch
f_confirm_registered_user_by_email


## Modeling and Other Documents
Not applicable


## Tasks

1. Create route for registration confirmation
2. Save registration status to database
3. Display registration confirmation page to user
