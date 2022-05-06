Feature: VA244

A short summary of the feature

@tag1
Scenario Outline: Create a vote page has a select list for timezones
	Given I am on the '<Page>' a vote page
	Then I will see a select list for timezones
	Examples:
	| Page   |
	| Create |

Scenario Outline: Logged in user will see Start Vote Now button and Start Vote Future button
	Given I am on the '<Page>' a vote page
	Then I will see a select list for timezones
		And Pacific Standard Time will be selected by default
	Examples:
	| Page   |
	| Create |