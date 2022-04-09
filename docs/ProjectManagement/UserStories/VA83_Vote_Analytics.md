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
See Vote Analytics UI MockUp under Modeling folder 

## Tasks
1. Add buttons in table on vote review page that say "vote analytics"
2. Add action method in create controller that redirects from vote review page to vote analytics that pass that vote id 
3. Create vote analytics html page 
4. Add Vote Analytics title to html vote analytics page 
5. Create vm for vote analytics page 
6. Add vote title and description to vm, and set it in create controller with passed in vote id 
7. Add this "<script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>" to layout.cshtml in head to use for charts (following example from: https://developers.google.com/chart/interactive/docs/gallery/piechart)
8. in vote analytics view model add list for storing vote totals and list for storing vote options 
9. Create repo methods for adding to those lists 
10. Test these repo methods 
11. Set items in create controller with these repo methods  
12. On analytics html add javascript code to scripts section in order to get pie charts to work correctly 
13. Add place in html page for where pie chart will go 
14. Manually test the interface and make sure pie chart looks ok, may need to go back and add in any needed styling of pie chart 
15. Add in back to vote review page button and call action method that has already been created to go back to that page 
16. In html page create if statement where lists of submitted votes is empty then tell user there are no votes for this created vote yet
17. Add in any needed tool tips for user to better understand what is happening 
18. Refactor table on created votes review page a bit to make it look nicer 

