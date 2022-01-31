# Yes/No Vote Creation
<hr>

## Title

*As a user I want to create a simple yes/no vote.*

## Description

This user story is about the creation of the yes/no vote. The user will be able to now click the yes/no vote option (with a description) on the first page. The confirmation page will show their description, as well as the type of vote they have chosen (being a yes/no vote).  

### Details:
1. Add "Yes/No" vote to the bullet list of voting types on create page with a description that states "Voters will get to choose between two simple options: yes or no."
2. When clicking submit users will be directed to the confirmation page that says below "Vote Creation Confirmation" in a smaller font "You have chosen to create a yes/no vote (Hit back if this was not the vote you wanted)"   
3. Below the vote description area will say "Vote Options"
4. Below vote options will have a bullet list that has yes and no 
5. After clicking return on the confirmation page the user will be directed to the home page 

## Acceptance Criteria
No .feature file for this one

    Given I am on the "Home" page 
    When I click on "Create A Vote" page 
    Then I will see "Yes/No" on the voting options list with a good description of what this means

    Given I am on the "Confirmation" page 
    Then I should see a header stating "You have chosen to create a Yes/No vote" 

    Given I am on the "Confirmation" page 
    And I click the back button 
    Then I should be brought back to the "Create A Vote" page 

    Given I am on the "Confirmation" page 
    And I have chosen a yes/no vote 
    Then I will see a "vote options" area with a bullet list that has yes and no 

## Assumptions/Preconditions
Vote creation page has been built

## Dependencies
Vote Creation Page

## Effort Points
2
## Owner

## Git Feature Branch
f_yes_no_vote_creation

## Modeling and Other Documents
See first and third pages of CreateVoteUI mockup in team repo under UIMockups

## Tasks
1. ...
2. ...