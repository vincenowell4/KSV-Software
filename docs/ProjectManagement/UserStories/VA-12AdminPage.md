# Admin Page View Votes


## Title

As an admin I want to be able to access the admin page, so I can see all created votes in the system

## Description

We want to have admin accounts be able to see that they are logged into an admin account rather than a normal account so we know that roles are enabled. An admin page is desired that is only accessable to admin accounts. On the admin page we want to see all the created votes that are on the system.


### Details
1. admin page when logged into an admin account
2. admin page not available when not logged into an admin account
3. admin will be able to see all created votes in the system

## Acceptance Criteria

    Given I am the administrator
    And I am logged in
    When I navigate to '/admin'
    Then I will see a welcome message

    Given I am not the administrator
    And I am logged in
    When I navigate to '/admin'
    Then I will not be able to see the welcome message

    Given I am a visitor and have no account
    When I navigate to '/admin'
    Then I will not be able to see the welcome message

    Given I am the administrator
    And I am logged in
    When I navigate to '/admin'
    I will be able to see a list of all Created votes

    Given I am a visitor and have no account
    When I navigate to '/admin'
    Then I will not be able to see a list of all Created votes

    Given I am not the administrator
    When I navigate to '/admin'
    Then I will not be able to see a list of all Created votes





## Assumptions/Preconditions
Blank MVC Project is created

## Dependencies
Create Blank Project
Login User
Register User

## Effort Points
2
## Owner
Sam Torris
## Git Feature Branch
f_add_authentication

## Modeling and Other Documents
N/A

## Tasks
1. ...
2. ...