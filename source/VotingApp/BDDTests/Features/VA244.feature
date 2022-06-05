Feature: VA244

A short summary of the feature

@tag1
Scenario Outline: Create a vote page has a select list for timezones
	Given I am on the '<Page>' a vote page
	Then I will see a select list for timezones
	Examples:
	| Page   |
	| Create |

Scenario Outline: Check that PST is default
	Given I am on the '<Page>' a vote page
	Then I will see a select list for timezones
		And Pacific Standard Time will be selected by default
	Examples:
	| Page   |
	| Create |