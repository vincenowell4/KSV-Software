# ID VA-83
<hr>

## Title

*As a logged in user I want an analytics page for my created vote so that I can see the distribution of voting results represented by visual analytics so I can interpret the results and make decisions easier.*

## Description

This user story is about a logged in user being able to see visual analytics for their created vote so they are able to interpret the results easier. For this we will want the logged in user to easily be able to choose what vote they wish to see analytics on, so we will add a vote analytics button on the vote review page next to each of their created votes. This button will bring them to a vote analytics page where they will easily be able to see the title of the vote they had just created. Below the title we will show them the overall results of their created vote with the use of a pie chart. Each voting option will be represented by a different color in the pie chart with the percentage and name of that voting option shown to the user. Below the chart will have a back to vote review page button in case the user would like to go back to that page. 

### Details:

1. View results page: add vote analytics button next to each created vote in the table 
2. Vote analytics page: add Vote Analytics title to the page  
3. Vote analytics page: add the vote title of the vote they clicked on 
4. Vote anlalytics page: a pie chart should be displayed that contains the vote results (if there are no votes a message will state that there are no votes yet). This pie chart should include each vote option that contains a vote that is labeled with that vote option name and the percentage of votes (compared to the total votes) of that option. Each option will be a different color so the user can clearly see the different voting options. 
5. Vote analytics page: add a button below the pie chart that says "back to vote review" which will bring the user back to the previous page they were on 

See the UI mockup under modeling and other docs for a quick idea of what it will look like 

## Acceptance Criteria
No .feature file for this one

    Given I am on the vote review page 
    When I click on the vote analytics button next to the option I want  
    Then I will be redirected to the vote analytics page 

    Given I am on the vote analytics page 
    Then I will see Vote Analytics at the top of the page 

    Given I am on the vote analytics page 
    Then I will see the title of that previous vote I had clicked on 

    Given I am on the vote analytics page 
    Then I will see a pie chart with the results of my vote 

    Given I am on the vote analytics page 
    And I have different voting options with votes 
    Then I will see each voting option on the pie chart with a different color 

    Given I am on the vote analytics page 
    And I have different voting options with votes 
    Then I will see each voting option with the name of the voting option  

    Given I am on the vote analytics page 
    And I have different voting options with votes 
    Then I will see each voting option with the percentage of that vote to the total votes    

    Given I am on the vote analytics page
    And my vote has 0 overall votes  
    Then I will see a message saying there are no votes yet  

    Given I am on the vote analytics page 
    And I click on the back to vote review button 
    Then I am brought back to the vote review page 

## Assumptions/Preconditions
Voting results page has already been completed in previous sprint. There is a user account that has created votes with votes in order to test functionality of this user story. 

## Dependencies
None (no other user stories in this sprint) 

## Effort Points
4

## Owner
Kaitlin

## Git Feature Branch
f_va83_vote_analytics_page

## Modeling and Other Documents
Vote analytics mock up page: [Vote Analytics Page](VA-83VoteAnalyticsUIMockup.png "Mockup of the vote analytics page")

## Tasks
1. Add buttons in table on vote review page that say "vote analytics"
2. Add action method in create controller that redirects from vote review page to vote analytics that pass that vote id 
3. Create vote analytics html page 
4. Add Vote Analytics title to html vote analytics page 
5. Create vm for vote analytics page 
6. Add vote title to vm, and set it in create controller with passed in vote id 
7. To create the pie chart I will be following the steps from this micr doc: https://docs.microsoft.com/en-us/aspnet/web-pages/overview/data/7-displaying-data-in-a-chart
8. Start by creating an array of the voting options - create a method in VoteOption repo that returns array of strings of the voting options for the passed in created vote id 
9. Test this new repo method 
10. Create an array of ints for the votes - create a method in SubmittedVotes repo that returns array of ints of the votes (grouped by the voting options) for the passed in created vote id 
11. Test this new repo method 
12. In vote analytics view model create two fields for the array of vote options and array of submitted votes - set these in the controller for the action method that returns the vote analytics page 
13. Create a new cshtml page for the creating of the chart first and fill it out with the vote analytics view model 
14. In the vote analytics html page add the place for the pie chart. To add in the pie chart we will add an image tag where we will put the cshtml page where we created the pie chart  
15. Manually test the interface and make sure pie chart looks ok, may need to go back and add in any needed styling of pie chart 
16. Add in back to vote review page button and call action method that has already been created to go back to that page 
17. In html page create if statement where array of submitted votes is empty then tell user there are no votes for this created vote yet
18. Add in any needed tool tips for user to better understand what is happening 

