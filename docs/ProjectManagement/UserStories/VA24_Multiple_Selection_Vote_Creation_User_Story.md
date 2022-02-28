# Multiple Selection Vote Creation
<hr>

## Title

*As a user I want to initiate a multiple selection vote so that I can see what my options will look like.*

## Description

This user story is about the user understanding the process of creating a simple multiple selection vote. The user would like to be able to enter in their options for the multiple selection choice and be able to add or delete options as well. After this they want to be able to confirm all of the details of their vote. On the confirmation page they would like to clearly see that they have chosen a multiple selection vote with the choices they had previously entered and their voting description. They also would like to be able to go back and change options if they realize they would like to change something, and be able to also go back to change the type of vote as well in case they realize that's not the type of vote they wanted.  

### Details:
1. Add "Multiple Selection" (make sure alphabetized still) vote to the bullet list of voting types on create page with a description that states "Voters will get to choose between the multiple options you enter on the next page (anywhere from 2-10 voting options will be allowed)" 
2. When clicking the "submit" button the user will be directed to a page where they can enter in the voting choices 
3. At the top of this voting options page will say "Enter Voting Options Below" 
4. In a smaller font below that will say "Enter at least two voting options, to add more than two click the add another option button"
4. There will be text boxes labeled with the option 1, option 2, etc. above each text box for the user to enter their voting options
5. Next to each voting option will have a delete button 
6. Below all the text boxes will say "If you have added too many voting options, and would like to delete one hit the delete button next to the option you don't want"
7. The back button from the voting options page will bring the user to the create a vote page 
8. After clicking submit on the voting options page the user will be redirected to the confirmation page 
9. On the confirmation page below "Vote Creation Confirmation" will now state "You have chosen to create a multiple selection vote"  
10. Under "Vote Options" will now have a bullet list of the voting options the user had entered on the previous screen  
11. After clicking return on the confirmation page the user will be directed to the home page 

## Acceptance Criteria
No .feature file for this one

    Given I am on the "Home" page 
    When I click on "Create A Vote" page 
    Then I will see "Multiple Selection" on the voting options list with a good description of what this means

    Given I am on the "Create A Vote" page 
    When I click on "submit"
    Then I will be redirected to the "Voting Options" page

    Given I am on the "Voting Options" page 
    And I click the back button 
    Then I should be brought back to the "Create A Vote" page

    Given I am on the "Voting Options" page 
    Then I will see "option 1" with a text box and "option 2" with a text box 

    Given I am on the "Voting Options" page 
    And I want to enter more options than the 2 to begin with
    When I can click the add another option button
    Then another option will pop up with a text box 

    Given I am on the "Voting Options" page 
    And I want to enter more than 10 options
    Then it will not add more than 10 options boxes 

    Given I am on the "Voting Options" page 
    And I only enter one option 
    When I hit the submit button
    Then it will stay on this page and say "Please enter at least two options"

    Given I am on the "Voting Options" page 
    And I have decided I don't want some of the voting options I have entered
    When I click the delete button
    Then that voting option will be deleted 

    Given I am on the "Voting Options" page 
    And I have finished entering all my options
    When I clikc the submit button
    Then I will be redirected to the confirmation page 

    Given I am on the "Confirmation" page 
    Then I should see a header stating "You have chosen to create a Multiple Selection vote" 

    Given I am on the "Confirmation" page 
    And I click the back button 
    Then I should be brought back to the "Voting Options" page 

    Given I am on the "Confirmation" page 
    And I have chosen a multiple selection vote 
    Then I will see a "vote options" area with a bullet list that has all of the options I have entered from the previous page 


## Assumptions/Preconditions
Vote creation page has been built

## Dependencies
Vote Creation Page

## Effort Points
4
## Owner

## Git Feature Branch
f_va24_multiple_selection_vote_creation

## Modeling and Other Documents
See 1, 2, 3 pages of CreateVoteUI mockup in team repo under UIMockups

## Tasks
1. Add multiple selection description to card on create a vote page 
2. Add multiple selection page to view 
3. Create action methods for multiple selection page and refactor any other actions methods if needed 