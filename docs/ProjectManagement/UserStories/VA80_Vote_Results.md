# Voting Results
<hr>

## Title

*As a logged in user who created a vote, I want to be able to see the results of the created vote after creation, so that I can make decisions based on the results.*

## Description

This user story is about a logged in user being able to see the results of their created vote. The results should be displayed on it's own page in an easy way for a user to see the results. This page should show the user the vote title, vote description, vote type, vote options, if it was anonymous, and the vote results. The user will be able to access this vote results page from the vote review page. 

### Details:
1. Add Vote Results page 
2. Add Voting Results button on review page next to each vote 
3. Add that chosen vote title and description on voting results page at the top 
4. Add voting results to that page (overall vote results count) for each vote option in a nicely formatted table 
5. Back to vote review button that brings them back to review page in case they want to look at their other created votes 
6. Add tool tips where needed to make things clear for user  

## Acceptance Criteria
No .feature file for this one

    Given I am on logged in
    When I go to the vote review page 
    And I click on the Voting Results button 
    Then I will be brought to the vote results page 
    
    Given I am on the vote review page
    And I click the vote results button next to the created vote I want the results for 
    When I am brought to the voting results page 
    Then I am able to see that vote title
    
    Given I am on the vote review page
    And I click the vote results button next to the created vote I want the results for 
    When I am brought to the voting results page 
    Then I am able to see that vote description

    Given I am on the vote review page
    And I click the vote results button next to the created vote I want the results for 
    When I am brought to the voting results page 
    Then I am able to see the results of that vote nicely formatted in a table next to each voting option

    Given I am on the voting results page
    And no one has voted yet
    Then I will see 0 next to the voting options in the table 

## Assumptions/Preconditions
There are votes for created votes 

## Dependencies
vote review page 

## Effort Points
4
## Owner
Kaitlin

## Git Feature Branch
f_va80_vote_results

## Modeling and Other Documents

## Tasks
1. Add vote results button next to each created vote on vote review page 
2. Add outline of vote results page so that you can test button is working to redirect to that page 
3. Add view model for vote results 
4. Add action method(s) in create controller to redirect from view results page to vote results page with created vote that was clicked on
5. Add SubmittedVoteRepository and ISubmittedVoteRepository 
6. Add method(s) needed to get all submitted votes
7. Test getting all submitted votes repo method(s)
8. Add any needed tool tips 
9. Add button on vote results page that brings user back to vote review page 
10. Add action method to button to make it work