# Confirm Account Via Email
<hr>

## Title

*As a user I want to be able to confirm my account via email, so that I can login after registering.*

## Description

This user story is about the user being able to login to their account after creation by confirming with email. A user will be able to click the link in their email that will confirm their account. After confirming their account they will be redirected to a registration confirmation page. This registration confirmation page will let the user know that their account with this email has been confirmed.  

### Details:
1. Once a user clicks the link in their email the account attached to that email will be confirmed 
2. The link will redirect the user to registration confirmation page 
3. The page will have a header that states "Your registration has been confirmed" 

## Acceptance Criteria
No .feature file for this one

    Given I click on the link in my email
    When I go back to login 
    Then I am able to log into my account 

    Given I click on the link in my email
    Then I should be redirected to the registration confirmation page 

    Given I am on the registration confirmation page 
    Then I should see a message stating that my registration has been confirmed 

## Assumptions/Preconditions
A user is able to register by email and has receieved confirm email 

## Dependencies
va-14

## Effort Points
4
## Owner
Kaitlin 

## Git Feature Branch
f_va60_confirm_account_by_email

## Modeling and Other Documents


## Tasks
1. ...