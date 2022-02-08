# Register User
<hr>

## Title

*As a user I want to go to the Voting App web site and register as a user, using my email address.*

## Description

This user story is about registering a user in the web site. Once registered, the app should send an email to the user.

### Details:

1. Registration page asks for user's first and last name, an email and a password. The standard Identity password policy will be used.
2. Once the user enters their info, and the fields are validated and acceptable, a confirmation email will be sent to the user's email address.

## Acceptance Criteria

    Given I am on the "Home" page 
    Then I will see a link to Register 

    Given I am on the "Home" page
    Then if I click on the Register link, it will tale me to the Register page

    Given I am on the "Register" page
    Then I should see a form with First Name, Last Name, Email, Password and Confirm Password fields and a Submit button

    Given I am on the "Register" page
    Then if I click Submit without entering information in any of the form fields and click Submit, I should see error messages telling me the fields are required

    Given I am on the "Register" page
    Then if I enter an email that is not in a valid format, I should see an error message telling me the email is not valid

    Given I am on the "Register" page
    Then if I enter a password that does not conform the the standard Identity password policy, I should get an error message telling me the password policy

    Given I am on the "Register" page
    Then if I enter Password and Confirm Password fields that do not match, I should see an error message that the Password and Confirm Password fields must match

    Given I am on the "Register" page
    Then is I enter valid data in all the fields and click Submit, then I should be take to a "Thank you for Registering" page, and I should receive a confirmation email from the application



## Assumptions/Preconditions
Initial project has been set up with the authentication database 


## Dependencies
Create blank project


## Effort Points
2


## Owner
Vince Nowell


## Git Feature Branch
f_register_user


## Modeling and Other Documents
Not applicable


## Tasks

1. Create or customize Register page
2. Validation for first and last name, email and password fields
3. using email API to send confirmation email to user



###
