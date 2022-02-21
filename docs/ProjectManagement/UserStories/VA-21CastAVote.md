# Vote Access Code


## Title

As a user, I want to be able to cast access a vote with an the access code given to the vote creator, so I can submit a vote.

## Description
We want the users to be able to access a vote that has been created by inputing an access code that gets generated when a vote is created.
We then want the user to be able to cast a vote on that created vote after entering the code.


### Details
1. Unique code for the created vote
2. should be placed in the database
3. Displayed to the user at the end of creating a vote
4. User should be able to view the vote by entering the code
5. User should be able to submit a choice on the vote after entering the code


## Acceptance Criteria

    Given im on the Vote Confirmation page.
    I will see a uniquely generated code for the vote.

    Given im on the Home page
    I will see a Submit A Vote nav item.

    Given im on the Home page
    When I select the Submit A Vote nav item.
    I should be taken to the Enter Vote Code page

    Given im on the Enter Vote Code page
    When I see a text box to enter in a code
    
    Given im on the Enter Vote Code page
    And I enter a valid code into the text box and hit enter
    I will be taken to the Submit a Vote Page

    Given im on the Enter Vote Code page
    And I enter an invalid code into the text box and hit enter
    I will recive an error message and be asked to reenter the code

    Given im on the Submit a Vote page
    I will see the vote discription and the voting options
    
    Given im on the Submit a Vote page
    and I dont make a selection and submit
    I will recive an error message telling me to choose an option

    Given im on the Submit a Vote page
    and I make a selection and submit
    I will taken to a confirmation page with my selected vote


## Assumptions/Preconditions


## Dependencies
Vote Creation Page
Simple Vote

## Effort Points
4
## Owner
Sam Torris
## Git Feature Branch
f_create_vote_access_code

## Modeling and Other Documents
Refer to Create Vote UI Mockup

## Tasks
1. ...
2. ...