# Create Login Page


## Title

*As a registered user I want to go to the Voting App web site and log into the app, using my email address and password.*

## Description

This user story is about logging into the Voting App, once a user is registered and confirmed.

### Details:

1. Login page asks for the user's email and password, and has a Submit button.
2. Once the user enters their email and password, and clicks Submit, the app will attempt to authenticate the user
3. If the user has not entered valid credentials, the app will display an error message
4. If the user has ented valid credentials, the app will return to the home page, and show the logged in user in the upper right of the page


## Acceptance Criteria

    Given I am on the "Home" page 
    Then I will see a link to Log In

    Given I am on the "Home" page
    Then if I click on the Log In link, it will take me to the Log In page

    Given I am on the "Log In" page
    Then I should see a form with Email and Password and a Submit button

    Given I am on the "Log In" page
    Then if I click Submit without entering information in any of the form fields and click Submit, I should see error messages telling me the fields are required

    Given I am on the "Log In" page
    Then if I enter an email that is not in a valid format, I should see an error message telling me the email is not valid

    Given I am on the "Log In" page and enter invalid credentials
    Then I should get an error message telling me the credentials are invalid

    Given I am on the "Log In" page
    Then if I enter a valid email and password for a user that is registered and confirmed, I should be taken back to the Home page, and see my email in the upper right of the Home page

    

## Assumptions/Preconditions
Initial project has been set up with the authentication database 


## Dependencies
Create blank project


## Effort Points
2


## Owner



## Git Feature Branch
f_create_login_page


## Modeling and Other Documents
UI Login Page diagram


## Tasks

1. Create or customize login page
2. Validation for email and password fields
3. Home page should show logged in user, once user is validated
