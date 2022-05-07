Feature: VA111_Kaitlin

*As an admin, I want activity on the site with logging errors and logging information, so that I am able to easily fix any issues and know what page they are coming from as well as see information from various pages.*


Background:
	Given the following users exist
	  | UserName | Email                   | FirstName | LastName | Password    |
	  | khaley18 | khaley18@mail.wou.edu   | Admin     | Admin    | Soccer3399! |
@tag1
Scenario: Admin can see drop down button for logging
	Given I am a user with first name '<FirstName>'
		And I login
	When I go to the Admin page 
	Then I will see a button for logging info
	Examples:
	| FirstName |
	| Admin   |

Scenario: Admin can see table for logging
	Given I am a user with first name '<FirstName>'
		And I login
	When I go to the Admin page 
	And I click on the logging button
	Then I will see a table with logging info
	Examples:
	| FirstName |
	| Admin   |
