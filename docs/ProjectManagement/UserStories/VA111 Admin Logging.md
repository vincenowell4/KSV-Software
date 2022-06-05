# ID VA-111
<hr>

## Title

*As an admin, I want activity on the site with logging errors and logging information, so that I am able to easily fix any issues and know what page they are coming from as well as see information from various pages.*

## Description

This user story is about an admin user being able to see logging errors and logging information. This way, they are able to track any errors and see what page it's coming from. They are also able to see the different logging information on the various pages as well. This will be useful information for bugs especially. To accomplish this task we want to add a table to the admin page with logging so the admin is able to see logging right away. This table will include the date, if it's an error or information, and the message for the log. The table we want organized by newest to oldest date of log so that the newest information is shown first for the admin.  

### Details:
1. Add a button on the admin page where they can click to see logging 

## Acceptance Criteria
No .feature file for this one

    Given I am an admin 
        And I am on the admin page 
    Then I should see a collapsible button for logging errors 

    Given I am an admin 
        And I am on the admin page 
    And I click on the collapsible
    Then I should see a logging table 

    Given I am an admin 
        And I am on the admin page 
    And I see a table for logging
    Then it should be in order by descending date 

     Given I am an admin 
        And I am on the admin page 
    And I see a table for logging
    Then I should see a logging message

    Given I am an admin 
        And I am on the admin page 
    And I see a table for logging
    Then I should see the log type (bug or informational)



## Assumptions/Preconditions
Admin account and page has already been created 
 
## Dependencies
None (no other user stories in this sprint) 

## Effort Points
4

## Owner
Kaitlin

## Git Feature Branch
f_va111_admin_logging

## Modeling and Other Documents
See Admin page UI MockUp under Modeling folder 
See VotingAppClass Diagram under Modeling folder   

## Tasks
1. Run down script
2. Update up script with new table and run up and seed 
3. Update old down script to include new table
4. Scaffold db 
5. Create App log repo in DAL folders
6. Add method in app log repo for log errors
7. Add method in app log repo for log info
8. Create method for add/update to AppLog in db
9. Create view model for List of AppLog
10. Add method in app log repo to get list of app log (ordered by date)
11. Add vm in admin controller - get all list of logs, return vm 
12. In all controllers add call to bug and info method in appropriate places
13. In admin index create drop down for table 
14. On index page add view model to the top and table for logs  
15. Test repo methods 
16. BDD tests