Feature: VA86

**As a user, I want make a vote delayed, so that I can make it now and have it active for voting later**

This user story is about adding the capability of creating a vote and delaying the start of voting on it until a future time. 
This will allow persons who are voting on an issue to have time to research and consider pros and cons of their choice, before
having the ability to submit their vote.

We will want to add two options and a date picker to the Create A Vote page; Start Vote Immediately and Start Vote at a 
Future Time, with the date picker following the second option.

If the user chooses Start Vote Immediately, then the date picker after Start Vote at a Future Time will be disabled.

If the user chooses Start Vote at a Future Time, then the date picker after Start Vote at a Future Time will be enabled.

The date picker following Start Vote at a Future Time, if enabled, will not allow a date/time in the past to be entered.

The app will automatically inform the vote creator by email when the vote start time has been reached, providing 
the vote title, description and vote access code in the body of the email.


	
Background:
	Given the following user exists
	  | UserName | Email               | FirstName | LastName | Password     |
	  | sam2     | sam2@mail.com       | sam       | 2        | Password123! |
	  #All passwords are local and are not actual passwords 
	And the following user does not exist
	  | UserName | Email               | FirstName | LastName | Password     |
	  | AndreC   | colea@example.com   | Andre     | Cole     | 0a9dfi3.a    |



@tag1
Scenario Outline: Non-logged in user can't create future vote time
	Given I am a user that isn't logged in
	When I navigate to https://localhost:7297/Create
#	Then I will not see the 
	Then I am redirected to the '<Page>' page
	  And I can see a personalized message in the navbar that includes my email
	Examples:
	| FirstName | Page |
	| sam   | Home |
