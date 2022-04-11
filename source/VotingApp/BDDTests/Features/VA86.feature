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
	  | vnowell21| vnowell21@wou.edu   | Vince     | Nowell   | Password123! |
	  #All passwords are local and are not actual passwords 
	And the following user does not exist
	  | UserName | Email               | FirstName | LastName | Password     |
	  | pjonesl  | pjones@example.com  | Paul      | Jones    | 0a9dfi3.a    |


Scenario Outline: Logged in user will see Start Vote Now button and Start Vote Future button
	Given I am a user that is logged in
	When I navigate to '<Page>' page
	Then I will see the Start Immediate Vote button
	  And I will see the Start Future Vote button
	Examples:
	| FirstName | Page   |
	| Vince     | Create |

Scenario Outline: Logged in user clicks Start Immediate Vote and Future DateTime is disabled
	Given I am a user that is logged in
	When I navigate to '<Page>' page
	  And I click on Start Immediate Vote
	Then the Future DateTime textbox will be disabled
	Examples:
	| FirstName | Page   |
	| Vince     | Create |

Scenario Outline: Logged in user clicks Start Future Vote and Future DateTime is enabled
	Given I am a user that is logged in
	When I navigate to '<Page>' page
	  And I click on Start Future Vote
	Then the Future DateTime textbox will be enabled
	Examples:
	| FirstName | Page   |
	| Vince     | Create |

Scenario Outline: Logged in user enters a Future DateTime then clicks Start Immediate Vote and Future DateTime is blanked out
	Given I am a user that is logged in
	When I navigate to '<Page>' page
	  And I click on Start Future Vote
	  And I enter a Future Date
	  And I click on Start Immediate Vote
	Then the Future DateTime textbox will be cleared
	Examples:
	| FirstName | Page   |
	| Vince     | Create |