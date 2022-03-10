# Individual Voting Results For Non-Anonymous Vote
<hr>

## Title

*As a user I want to be able to see who voted on my vote and what they chose if anonymous is not selected when creating the vote.*

## Description
This user story is about a user being able to see the individual results of their created vote given a vote is not anonymous. They will be able to see who voted for what voting option this way a user can see how individuals voted. If it's not anonymous then a user won't be able to see how individuals voted (just the overall result).  

### Details:
1. Add if the vote was anonymous or not at the top of vote results page 
2. If vote is not anonymous then users will get to see a table below the overall results with the individuals voting results
3. The voting results will show the users email (if they were logged in) next to the option they chose 
4. The results will also have a spot for each option that was voted on by users not logged in with the total number of users not logged in who voted for that result 

## Acceptance Criteria
No .feature file for this one

    Given I am on the vote results page 
    And my vote is not anonymous  
    Then I will see how users voted 

    Given I am on the vote results page
    And my vote is not anonymous 
    Then I will see the email of users who were logged in next to voting option they voted for 

    Given I am on the vote results page
    And my vote is not anonymous 
    Then I will see the total number of users who were not logged in next to what they voted for 

    Given I am on the vote results page
    Then I will see if the vote was anonymous or not at the top 

    Given I am on the vote results page 
    And my vote is anonymous  
    Then I will not be able to see how users voted

## Assumptions/Preconditions
Users can cast votes on already created votes

## Dependencies
Vote results page is created 

## Effort Points
4

## Owner
Kaitlin

## Git Feature Branch
f_va81_how_users_voted_when_not_anonymous

## Modeling and Other Documents


## Tasks
1. When anonymous box is checked on create a vote page set to anonymous or not 
2. Add anonymous field to confirmation vm and set in create controller 
3. Add anonymous to table on vote review page 
4. Add anonymous field to vote results vm and set in create controller
5. Display if created vote was anonymous or not at the top of vote results page 
6. Make method for getting list of votes for users that were logged in (submitted vote repo) and add to vote results vm and create controller 
7. Make method for getting totals of each vote for users that were not logged in (submitted vote repo) and add to vote results vm and create controller 
8. Test these repo methods added
9. Create table in vote results html page for users that were logged in and total for users that were not logged in 
10. Add in any needed tool tips 
11. Add total votes to table on vote results page 
12. Add field in vm that holds total submitted votes
13. Create repo method for getting total votes 
14. Test total votes method 
15. Add in repo method to get winner (or if there is a tie)
16. Add field in view model for winner
17. Add in create controller 
18. Test that winner repo method 
19. Add winner to html page 