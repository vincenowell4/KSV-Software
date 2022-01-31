# Confirm Registered User By Text


## Title

*As a user I want to confirm that I am a registered user of the Voting App web site by clicking the link sent by the app to my cell phone, via a text message.*

## Description

This user story is about confirming the registration of a user in the web site. If the user clicks the link in the text message sent by the app to their cell phone, it should take them back to the Voting App web site, mark in the database that they are a registered user, and display a page that indicates that the user has confirmed their registration in the Voting App. 

### Details:

1. The user will click the link in the text message sent by the Voting App to the cell phone number of the user entered on the Registration page.
2. The link will take the user back to the Voting App, save their registration status in the database, and display a page that indicates that the user has confirmed their registration in the Voting App. 

## Acceptance Criteria
No .feature file for this one

    Given I have received a confirmation text from the Voting App
    Then if I click on the link in the text, I should be taken back to the Voting App web site, which should display a  page confirming that I am a registered user of the Voting App


## Assumptions/Preconditions
Initial project has been set up with the database


## Dependencies
Create blank project


## Effort Points
1


## Owner



## Git Feature Branch
f_confirm_registered_user_by_text


## Modeling and Other Documents
Not applicable


## Tasks

1. Modify code used for confirming by email, if necessary 
