# Blank MVC Project


## Title

As a user when I would like a web app and homepage so I can get ready to use features of the application.

## Description

Build an empty ASP.NET Core Web app using .Net 6, and place a welcome message on the home page. We would also like to have the privacy page removed. When on the home page we should see the app name in the title.


### Details
1. Standard ASP.NET Core MVC application. Use HTTPS, no authentication.
2. Homepage with app title in corner
3. Welcome message on main page
4. Loaded with bootstrap


## Acceptance Criteria

    Given I am on the homepage
    Then I will see a navbar
    And I will see a banner image
    And I will see a welcome message 

    Given I am on the homepage
    Given I am on the homepage
    Then I will not see a privacy link on the navbar
    And I will not see a copyright message  

    Given I am on the homepage
    Then Bootstrap will be loaded
    And Bootstrap will be used for main content 


## Assumptions/Preconditions
None


## Effort Points
2
## Owner
Sam Torris
## Git Feature Branch
f_create_blank_project

## Modeling and Other Documents
Refer to Homepage UI Mockup

## Tasks
1. ...
2. ...