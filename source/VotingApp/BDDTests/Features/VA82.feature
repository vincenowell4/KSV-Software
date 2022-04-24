Feature: VA82

A short summary of the feature
Background:
	Given the following users exist
	  | UserName   | Email                 | FirstName | LastName | Password  |
	  | sam2     | sam2@mail.com    | sam    | 2    | Password123! |
@tag1
Scenario Outline: Audio Available On Submit A Vote Page
	Given I am on the 'Access' a vote page 
	When I enter in the '<AccessCode>'
	Then I will be navigate to the Submit a vote page for '<AccessCode>'
		And I will see an option to play audio for the vote
	Examples: 
	| AccessCode |
	| 16ba88    |

Scenario Outline: Audio Available On Vote Review Page
	Given I am a user with first name '<FirstName>'
		And I login
	When I navigate to the 'VoteReview' page 
	Then I will see audio available for each vote
	Examples:
	| FirstName |
	| sam   |

Scenario Outline: Audio Available On Vote History Page
	Given I am a user with first name '<FirstName>'
		And I login
	When I navigate to the Vote History page 
	Then I will see audio available for each vote
	Examples:
	| FirstName |
	| sam   |