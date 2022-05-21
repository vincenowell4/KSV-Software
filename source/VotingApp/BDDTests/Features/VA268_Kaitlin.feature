Feature: VA268_Kaitlin

*As a user I want an updated help page, so that I can get help on creating a vote and understand how the voting process works amongst all the different options.*

@tag1
Scenario: A user sees accordian for a multi-round vote 
	Given I am on the help page 
	Then I should see a multi-round vote accordian

Scenario: A user clicks on multi-round vote accordian and sees the directions
	Given I am on the help page  
	And I click on the multi-round vote accordian
	Then I should see the directions for a multi-round vote 

Scenario: A user clicks the create a poll button from the multi-round accordian directions and redirected to correct page 
	Given I am on the help page 
	And I click on the multi-round vote accordian
	And I click on the create a poll button
	Then I should be redirected to the Create page 

Scenario: A user sees FAQ title on the help page
	Given I am on the help page 
	Then I should see a FAQ title

Scenario: A user sees the FAQ cards on the help page 
	Given I am on the help page 
    Then I should different cards with FAQ
	