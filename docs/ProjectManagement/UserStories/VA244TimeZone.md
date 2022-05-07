# VA 244


## Title

*As a user I want to specify what timezone I want my vote to be in, so that the vote is local to my time zone*

## Description

In this user story we want to add in the ability for the user to specify the time zone they want the vote to be in. This will make it so that when a user creates a vote they can say what timezone they want it to be a part of, this will fix problems with users who are not in the default timezone or far away from getting confused about when votes will open or close

### Details:

1. When the user is on the create a vote page there will be a select list with timezones.
2. Based on the time zone picked the vote will be opened/closed based on that time zone

## Acceptance Criteria
    Given I am on the Create a Vote Page
    I will see a select list with timezones
    and it is defaulted to PST

    Given I am on the Create a Vote Page
    And I changed the timezone
    the vote open/close date will be in the time selected



## Assumptions/Preconditions
List of timezones items in Database or in Code

## Effort Points
4


## Owner
Sam Torris


## Git Feature Branch
f_add_timezone_to_create_a_vote_page_va244


## Modeling and Other Documents


## Tasks

1. Create new table in database for timezones with ID
2. Create new field in CreatedVote table for timezone
3. Create a select list on create a vote page
4. edit code to use the created vote selected timezone for evaluation