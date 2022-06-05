# ID VA-267
<hr>

## Title

*As an admin I want a custom error page for other users, so that our site is more secure.*

## Description

This user story is about a user getting a custom error page upon causing an error. This custom error page should not include any internal app information in order to be more secure. When a user is redirected to this error page we want it to have some type of message (can be something funny) about an error happening. There should also be a back to home page button so that the user can click that button and be brought back to the home page.  

### Details:
1. Create a custom error page 
2. Add some type of funny background and message to user
3. Let user know there was an error and apologize 
4. Add a back to home button so that the user will be brought back to the home screen of our site 

## Acceptance Criteria
No .feature file for this one

    Given I am a normal user 
        And I try to go to a page that doesn't exist
    Then I should see a custom error page 

    Given I am on the custom error page 
    Then there should be no internal application information

    Given I am on the custom error page 
    Then I should see a back to home page button

    Given I am on the custom error page 
    And I click the back to home page button
    Then I should be brought to the home page 

## Assumptions/Preconditions
None
 
## Dependencies
None (no other user stories in this sprint) 

## Effort Points
4

## Owner
Kaitlin

## Git Feature Branch
f_va267_custom_error_page

## Modeling and Other Documents
See error page UI MockUp under Modeling folder  

## Tasks
1. Create error controller 
2. Create action methods for 404 and 500 error pages (returning view)
3. Create 404 error page (adding image, text, back to home button)
4. Create 500 error page (adding image, text, back to home button)
5. Add back to home button method attached to home button on custom error pages 
6. In program.cs add so that if a 404, or 500 error occus to redirect to the appropriate error page 
7. Create a 500 error to test and make sure 404 error works as well 