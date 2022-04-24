# ID VA-206
<hr>

## Title

*As a user, I want to be able to copy the access codes for my vote and for private votes have the access code be automatically sent to authorized users, so that I can easily share the code and have them submit votes quickly.*

## Description

This user story is about a user being able to easily send out their vote access codes. This way they can easily copy and paste the code and message or email the code to anyone they want. The copy and paste button should be added on the vote confirmation page next to the access code url, as well as on the vote results page next to the url as well. This pasted URL will bring any user they want to the submit a vote page for that created vote. 

The second part of this story will allow the user to not have to send out the access codes to the authorized users themselves. After entering in the authorized users list of emails it will send out an email to all of these users. In the email it will contain the link to bring them to the submit a vote page where they will be able to easily submit their vote. 

### Details:

1. Add copy icon next to vote access url on confirmation page so that once a user clicks on this icon it correctly copies the url to vote on this created vote 
2. Add same copy icon next to the vote access url on the vote review page that copies currect url to vote 
3. When a user creates a private vote, after entering in the authorized emails and hits review send out an email to those users. Email will contain access link that will bring users to submit a vote page (they will still have to log in first)

## Acceptance Criteria
No .feature file for this one

    Given I am on the vote confirmation page 
    And I click the copy access vote code to clipboard button
    Then the correct url to vote on my created vote is copied 

    Given I am on the vote confirmation page
    And I see a vote access link
    Then I should see a copy button to copy the url 

    Given I have created a private vote
    And entered in my authorized user emails
    Then the authorized users should get an email

    Given I have received an email with the url 
    When I click the url then I am brought to the log in page
        And I am able to log in 
    Then I am able to cast a vote 

    Given I am on the vote review page 
    And I click the copy access vote code to clipboard button
    Then the correct url to vote on my created vote is copied  

    Given I am on the vote review page 
    And I see a vote access link
    Then I should see a copy button to copy the url

    Given I have created a private vote
    And I copy the access a vote url 
    When I accidently send it to someone who isn't authorized 
    Then that user is not able to submit a vote 


## Assumptions/Preconditions
Private votes have been set up properly  

## Dependencies
None (no other user stories in this sprint) 

## Effort Points
4

## Owner
Kaitlin

## Git Feature Branch
f_va206_easily_send_access_code

## Modeling and Other Documents
See Vote Review page UI MockUp under Modeling folder 
See Vote Creation Confirmation page UI Mockup under Modeling folder 

## Tasks
1. Add script to shared layout page so that proj can use already made icons (for copy icon)
2. fix bug for vote review page (when description is smaller than 5 chars it throws exception, add if statement in html page)
3. on vote confirmation page add copy icon 
4. Make it so that upon click of copy icon that the url is copied to clipboard (using js)
5. alert user when url is copied to clipboard
6. on vote review page add copy icon similar to vote confirmation page so that when clicked url is copied to the clipboard and alert user 
7. add js code to copy url buttons to clipboard 
8. in create controller check if vote is private - if it is then call repo method to send out emails
9. In repo method make sure email adds the title and description as well as a link to submit a vote  
10. Test repo method used to send email (test it gets authorized users correct, etc.)