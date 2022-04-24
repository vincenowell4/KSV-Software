# ID VA-109
<hr>

## Title

*As a user that is not logged in, I want to be able to see the voting results for my created vote, so that I am able to make decisions based on these results.*

## Description

This user story is about a user not logged in being able to see voting results for their created vote so they are able to make decisions on the results. The non logged in user will be able to easily access the voting results from the Access A Vote page, where they can easily enter the vote access code and be redirected to the voting results page. On this voting results page it will include the vote title and description as well as a table with the voting results.

The voting results table will include rows for each vote option and the total number of votes for that option. The winner will be highlighted with a color so the user can easily see which option won. If there is a tie then a different color will highlight the tied winning options. The user will also be able to see the total number of votes as well. There will also be a button below the table that will bring the user back to the access a vote page easily. Any needed tool tips will be added so that the user is able to better understand what is happening.

### Details:

1. Change navbar item to say Access a vote rather than submit a vote 
2. Change when they click access a vote to have two buttons - one that says Submit A Vote and the other will say Vote Results where they will enter in the access code 
3. Submit a vote will redirect to the page that was already created for submitting a vote, Vote Results will redirect to a vote results page 
4. If a user enters a code that is incorrect when trying to access the vote results page they will be told it's incorrect 
5. On Vote Results page: add title (so they know they are on vote results page)
6. On Vote Results page: add vote title from vote they are trying to access
7. On Vote Results page: add vote description from vote they are trying to access 
8. On Vote Results page: add voting results table - this table will include the vote option, total votes per option, as well as a row at the bottom for the total number of votes. The winning vote row will be highlighted. If there is a tie then the winning row options will be highlighted. Votes will also be displayed in table in descending order. 
9. On Vote Results page: add button under voting results page to bring them back to access a vote page  

## Acceptance Criteria
No .feature file for this one

    Given I am on the access a vote page
    Then I will see two buttons that say Submit A Vote and Vote Results   

    Given I am on the access a vote page
    And I enter a valid access code
    When I hit the Submit A Vote button
    Then I will be redirected to the submit a vote page 

    Given I am on the access a vote page
    And I enter a valid access code 
    When I click on the Vote Results button
    Then I will be redirected to the vote results page      

    Given I am on the access a vote page
    And I have entered a vote access code that is invalid 
    Then I will be told the code is invalid and to please try again

    Given I am on the vote results page 
    Then I will see a vote results header, vote title, and vote description 

    Given I am on the vote results page 
    Then I will see a vote results table that will include the vote option with the total number of votes for each option

    Given I am on the vote results page
    Then I will see the overall total number of votes in the bottom row of my table 

    Given I am on the vote results page 
    And there is only one winning option 
    Then I will see that winning option row highlighted

    Given I am on the vote results page 
    And there is only multiple winning options 
    Then I will see all the winning options rows highlighted 

    Given I am on the vote results page 
    And I click on the back to access a vote button 
    Then I am brought back to the access a vote page 

## Assumptions/Preconditions
There is a created vote with votes in order to test functionality of this user story. 

## Dependencies
None (no other user stories in this sprint) 

## Effort Points
4

## Owner
Kaitlin

## Git Feature Branch
f_va109_vote_results_users_not_logged_in

## Modeling and Other Documents
See Access A Vote page UI MockUp under Modeling folder 
See Vote Results Page UI Mockup under Modeling folder 

## Tasks
1. Change navbar to say Access a vote rather than submit a vote
2. Change title to say access a vote on page 
3. Add two input buttons for access and then submit a vote with submit buttons
4. Create action method that brings user to view results page when clicking submit for vote results (validate access code)
5. In action method if created vote is private make sure that only authorized users can access vote results 
6. Create view for results page 
7. Add needed fields in vm that's needed for this page 
8. Add in vote title and description 
9. Add everything needed on results page in order to get the results table 
10. Add back to access page button  
11. 4 BDD tests

