# Vote Creation Page
<hr>

## Title

*As a user I want to view a page where I am able to create a vote that is simple and easy to use.*

## Description

This user story is about the intial creation of the vote page. The page should be straight forward and easy for a user to understand the steps needed to take in order to create their vote. This page will just display the placeholder for the types of voting we will offer when creating a vote as well as a description box for the vote if the user would like to enter one. After clicking submit it will bring the user to a confirmation page. 

### Details:

1. Navbar with spot that says "Create A Vote"
2. This page should be built at `/create` 
2. Nice header that says "Create A New Vote" 
3. Below header (smaller size font than header) will say "Please enter your vote description in the box below (required)" with an empty text box below where the user can choose to enter a description if they would like to 
4. Below text box (same size font as step above) will say "Please choose which type of vote you would like to create" 
5. Below this text we will have a placeholder for a bullet list (alphabetically sorted) of all the voting options we have with descriptions below the voting type
6. For now, this list will remain blank until we do future user stories for each specific vote 
7. Below list of vote types we will have a submit button 
8. After clicking submit user will be redirected to a confirmation page 
9. Confirmation page will say: "Vote Creation Confirmation"
10. Below in a smaller font it will say "Vote Description (if you would like to change your description press the back button)" that will have the description the user had entered on the first page 
11. There will be a back button if the user would like to go back and change their vote description 
12. The bottom of the confirmation page will have a return button that brings the user back to the home page 

## Acceptance Criteria
No .feature file for this one

    Given I am on the "Home" page 
    When I click on "Create A Vote" page 
    Then I will see Create A New Vote at the top 

    Given I am on the "Create A Vote" page 
    When I see Please enter your vote description in the box below 
    Then I should be able to enter in a description for the vote in that text box

    Given I am on the "Create A Vote" page 
    When I see "Please enter your vote description in the box below"
    And I don't enter a vote description and hit submit
    Then I am asked to please enter a vote description before entering submit 

    Given I am on the "Create A Vote" page 
    And I am looking below the "Please enter your vote description in the box below" text box 
    Then I should see "Please choose which type of vote you would like to create" 
    And an empty bullet list placeholder of where the types of votes would be 

    Given I am on the "Home" page 
    When I navigate to the "Create A Vote" page
    Then the URL be at /create 

    Given I am on the "Create A Vote" page 
    And I enter my vote description 
    When I click the submit button 
    Then I should be redirected to a confirmation page 

    Given I am on the confirmation page 
    Then I should see a header that says "Vote Confirmation Page"

    Given I am on the confirmation page
    When I see "Vote Description"
    Then I should see the vote description I entered from the previous page 

    Given I am on the confirmation page
    When I see "Vote Description"
    And I want to change my description
    Then I can hit back and change my vote description

    Given I am on the confirmation page
    And I am finished with the creation of my vote
    When I click the return button
    Then I am redirected back to the home page 

## Assumptions/Preconditions
Initial project has been set up with db 

## Dependencies
Create blank project

## Effort Points
4
## Owner

## Git Feature Branch
f_vote_creation_page

## Modeling and Other Documents
See first page and third page of CreateVoteUI mockup in team repo under UIMockups

## Tasks
1. Write repository method to get all types of votes and sort them alphabetically 
2. Test after each method 
3. Once each test is passed, create controller for creation page with needed action methods 
4. Add navbar item for Create A Vote
5. Create view page for create 
6. Design create page matching format, styled nicely 
6. Test interface to make sure it's working correctly 