Feature: VA207_Kaitlin
*As a user when creating a vote and reviewing a vote I would like the steps to be easier to use and organized better so that creating and reviewing votes is an easier process.*

This user story is about organizing a few different pages that have become disorganized and confusing for a user. We want the user to have no trouble when creating a vote or reviewing voting results. 
We want to reorganize creating a vote for a user and the review a vote page especially.

@tag1
Scenario: User is able to see a collapsible for vote type descriptions
	Given I am on the create a vote page 
    Then I will see a collapsible for vote type descriptions

Scenario: When creating a vote user sees an area for required fields with vote title
	Given I am on the create a vote page
    Then I will see one area with a header labeled required
    And it will contain a vote title field
     
Scenario: When creating a vote user sees an area for required fields with vote description
	Given I am on the create a vote page
    Then I will see one area with a header labeled required
    And it will contain a vote description field

Scenario: When creating a vote user sees an area for required fields with vote type 
	Given I am on the create a vote page
    Then I will see one area with a header labeled required
    And it will contain a vote type field
