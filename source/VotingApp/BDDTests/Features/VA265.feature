Feature: VA265

A short summary of the feature
Background:
	Given the following users exist
	  | UserName   | Email                 | FirstName | LastName | Password  |
	  | sam2     | sam2@mail.com    | sam    | 2    | Password123! |
@tag1
Scenario Outline: When a poll has been voted on by the user, they will see a message saying they have casted a vote.
	Given I am on the 'Access' a vote page
	When I enter in the '<AccessCode>'
	Then I will be navigate to the Submit a vote page for '<AccessCode>'
		And I will see a message saying that I have already cast a vote
	Examples: 
	| AccessCode |
	| b14dac    |

Scenario Outline: Audio Available On Vote Review Page
	Given I am a user with first name '<FirstName>'
		And I login
	When I navigate to the 'VoteReview' page 
	Then I will see qr code available for each vote
	Examples:
	| FirstName |
	| sam   |