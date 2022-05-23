# VA 266


## Title

*As a user, I want a QR code for my vote, so that I can quickly share with others*

## Description

In this user story we want the app to create a QR code for each poll that gets created so that users
can share them easily by having others scan the QR code. This will have the user scan the code and then take them to the
submit a vote apge

### Details:

1. When a user creates a vote then on the confirmation page there will be a qr code for the user
2. When the user scans the qr code it will take the user to the cast a vote page for that vote

## Acceptance Criteria
    Given I am on the vote confitmation page
    Then I should see a QR code for the vote

    Given I am on the poll review page
    Then I should see a QR code for each vote

    Given I scan the QR code for the vote
    Then I should taken to the submit a vote page for that vote
## Assumptions/Preconditions
N/A

## Effort Points
4


## Owner
Sam Torris


## Git Feature Branch
f_add_qr_code_to_vote


## Modeling and Other Documents


## Tasks
1. Find qr code api
2. learn how to send qr code request
3. create method to send request with given data
4. create code to change returned method into byte array
5. add byte array item to db for the qr code
6. add byte array into db when qr code is created
7. create action method to change byte array into png for page