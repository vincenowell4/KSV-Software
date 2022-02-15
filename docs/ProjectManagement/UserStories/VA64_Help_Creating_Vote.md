# Help On Vote Creation
<hr>

## Title

*As a user, I want help on creating a vote, so that I understand how to use the site better.*

## Description

This user story is about making it easier for the user to understand the steps when creating a vote. Clarification for the process for a user is the main purpose. Button names will be fixed so the user will better understand what happens when clicked. Tool tips will be used on the user description for the user to better understand the purpose. Creation of a vote will be better organized with a title field as well as tool tips again for the user to understand the purpose of this as well. A help page will be added for users that need extra information and examples

### Details:
1. Change button on create a vote page to say "Review"
2. Change button on confirmation page to say "Submit" 
3. Add Title field on create a vote page 
4. Add info icons (tool tips) for title, descripton, and anonymous fields with better descriptions for user to understand purpose of 
these 
5. Add help to nav bar
6. Add help page with examples of current vote types (yes/no and multiple selection) 

## Acceptance Criteria
No .feature file for this one

    Given I am on the "Create A Vote" page 
    When I am done inputing the fields for create a vote 
    Then I will see a button at the bottom that says "Review"

    Given I am on the "Create A Vote" page
    Then I will see a title field to enter my title 

    Given I am on the "Create A Vote" page
    And I don't enter a title in the title field 
    Then I will see a message that this field is required 

    Given I am on the "Create A Vote" page
    And I click on the little information icon in the corner of the Title field
    Then I will see a message about what the purpose of the title field is for 

    Given I am on the "Create A Vote" page
    And I click on the little information icon next to the anonymous field
    Then I will see a message about what the purpose of the anonymous field is for 

    Given I am on the "Create A Vote" page
    And I click on the little information icon in the corner of the Vote Description field 
    Then I will see a message about what the purpose of the vote description field is for 

    Given I am on the "Vote Confirmation" page 
    When I am done reviewing my vote creation information 
    Then I will see a bottom at the bottom that says "Submit"

    Given I am on the "Home" page 
    Then I should see a navbar item for the help page 

    Given I am on the "Help" page
    Then I should see examples of yes/no and multiple selection votes 
 
## Assumptions/Preconditions
Vote creation page has been built

## Dependencies
Vote Creation Page

## Effort Points
2

## Owner
Kaitlin

## Git Feature Branch
f_va64_help_creating_vote

## Modeling and Other Documents

## Tasks
1. ...