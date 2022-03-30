# VA-84

## Title

*As a user I want my vote to only be accessed by those that have been given access, so that I can have a secure vote with only people that I allow to see it*

## Description

This user story is focused on allowing the user to enter in a list of user/emails that will be able to access the vote given that they have an account. We want it so that if a user is placed in the list they will be able to access the vote and submit a vote for that given item, but if they are not on the list of authorized users then they will not be able to access that given vote.

The input for users should allow the creator to easily add multiple users at once via an email. We want to user to be able to go back and add and remove people from the list before the voting has started.

A user that is authorized should be able to use the access code to access the vote or use the link that was shared given they are logged in. A user that is not logged in or is not authorized to submit a vote will see a message they they have not been authorized.

### Details:

1. The method for inputing the list of users should be easy allow for many users to be input at a given time
2. List of users that are authroized should be stored and be able to be changed
3. Non-authorized users should not be able to see the vote or cast a vote
4. After the voting window has opened authorized users should not be able to be edited or added


## Acceptance Criteria
      Given I am on the Create a Vote page
      Then I should see a place to enter a list of authorized users

      Given I am on the Create a Vote page
      And I enter in a list of user/users
      Then that vote should only be available to users specified

      Given I am on the Create a Vote page
      And I dont enter in a list of user/users
      Then that vote should be open to anyone with the access code

      Given I am on the Vote Confirmation Page
      And I have entered a list of authorized users
      Then I should see a the list of user entered

      Given I am on the Vote Confirmation Page
      And I have not entered a list of authorized users
      Then I should see that the vote is open to anyone

      Given I am on the access a vote page
      And I enter in a vote access code
      And I am an authorized user
      Then I should be taken to the submit a vote page

      Given I am on the access a vote page
      And I enter in a vote access code
      And I am not an authorized user
      Then I should see a message that I am not authorized

      Given I am on the access a vote page
      And I enter in a vote access code
      And I am not an authorized user
      Then I should see a message that I am not authorized

   

## Assumptions/Preconditions


## Dependencies
None - all necessary dependencies have already been completed

## Effort Points
4

## Owner
Sam Torris

## Git Feature Branch
f_VoteUserAuthorization_VA84

## Modeling and Other Documents

## Tasks
1. Modify database to allow for a list of users to be entered
2. Add input for list of user on create a vote page
3. Bind input to action method and place into database
4. Write code to authenticate user for accessing a vote
5. Add edit method for adding a removing users for authorization list