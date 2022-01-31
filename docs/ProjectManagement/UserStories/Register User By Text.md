# Register User By Text


## Title

*As a user I want to go to the Voting App web site and register as a user, using my cell phone number.*

## Description

This user story is about registering a user in the web site, using a cell phone number. Once registered, the app should send a text to the user.

### Details:

1. Registration page asks for user's first and last name, and have an option for either an email address or a phone number, and also a password field. The standard Identity password policy will be used.
2. Once the user enters their info, and the fields are validated and acceptable, a confirmation text will be sent to the user's cell phone.

## Acceptance Criteria

    Given I am on the "Home" page 
    Then I will see a link to Register 

    Given I am on the "Home" page
    Then if I click on the Register link, it will take me to the Register page

    Given I am on the "Register" page
    Then I should see a form with First Name, Last Name, Email, Phone Number, Password and Confirm Password fields and a Submit button

    Given I am on the "Register" page
    Then if I click Submit without entering information in any of the form fields and click Submit, I should see error messages telling me the fields are required

    Given I am on the "Register" page
    Then if I enter a phone number that is not in a valid format, I should see an error message telling me the phone number is not valid

    Given I am on the "Register" page
    Then if I enter a password that does not conform the the standard Identity password policy, I should get an error message telling me the password policy

    Given I am on the "Register" page
    Then if I enter Password and Confirm Password fields that do not match, I should see an error message that the Password and Confirm Password fields must match

    Given I am on the "Register" page
    Then if I enter valid data in all the fields and click Submit, then I should be take to a "Thank you for Registering" page, and I should receive a confirmation textemail from the application



## Assumptions/Preconditions
Initial project has been set up with the authentication database 


## Dependencies
Create blank project


## Effort Points
2


## Owner



## Git Feature Branch
f_register_user_by_text


## Modeling and Other Documents
Not applicable


## Tasks

1. Modify Register page to add phone number field
2. Validation for first and last name, phone number and password fields
3. using SMS text API to send confirmation text to user
