# VA-86

## Title

*As a user, I want to make a vote delayed, so that I can make it now and have it active for voting later.*

## Description

This user story is about adding the capability of creating a vote and delaying the start of voting on it until a future time. This will allow persons who are voting on an issue to have time to research and consider pros and cons of their choice, before having the ability to submit their vote.

We will want to add two options and a date picker to the Create A Vote page; Start Vote Immediately and Start Vote at a Future Time, with the date picker following the second option.

If the user chooses Start Vote Immediately, then the date picker after Start Vote at a Future Time will be disabled.

If the user chooses Start Vote at a Future Time, then the date picker after Start Vote at a Future Time will be enabled.

The date picker following Start Vote at a Future Time, if enabled, will not allow a date/time in the past to be entered.

The app will automatically inform the vote creator by email when the vote start time has been reached, providing the vote title, description and vote access code in the body of the email.

### Details:

1. Create A Vote Page will have three new fields: 2 radio buttons (Start Vote Immediately and Start Vote at a Future Time) and a date picker following the second radio button.
2. By default, Start Vote Immediately will be selected, so if the user doesn't click on Start Vote at a Future Time, then the vote will start as soon as it is created. By default, the date picker will be disabled, with no date/time showing in the text box.
3. If the user clicks Start Vote at a Future Time, then the date picker following it will be enabled, with no date/time showing in the date picker textbox.
4. If the user tries to click Review after selecting Start Vote at a Future Time but without selecting a date/time from the date picker, there will be a validation message by the date picker textbox stating "Vote Start Date/time is required".
5. If the user clicks Start Vote Immediately after clicking Start Vote at a Future Time and picking a future date time, then the date/time in the date picker textbox will revert back to a blank date/time, and the date picker will be disabled.
6. If, after clicking Start Vote at a Future Time, the user tries to enter a date/time in the past, there will be a validation message by the date picker textbox stating "Vote Start Date/Time must be in the future".
7. Votes created with the delayed vote time option will not initially have an access code generated in the CreatedVote table.
8. After successfully submitting a vote using the option to Start Vote at a Future Time, the app will have a newly created asynchronous process continually check for created votes marked as delayed that do not yet have a VoteAccessCode. For those votes, the VoteStartDateTime will be checked, and once that time is reached, then an access code will be generated and updated in the CreatedVote table.
9. This newly created process will, after generating the vote access code, generate an email to be sent to the vote creator, which will include the vote title, description and newly generated access code, and a reminder that votes can now be submitted.

See the UI mockup in Modeling and Other Documents, below.

## Acceptance Criteria
As a non-registered user or a registered user who is not currently logged in
If I navigate to the Create a Vote page
Then I will not see the options to Start Vote Immediately or to Start Vote at a Future Time

As a registered user who is currently logged in
If I navigate to the Create a Vote page
Then I will see the options to Start Vote Immediately or to Start Vote at a Future Time

As a registered user who is currently logged in
If I navigate to the Create a Vote page and I set the option to Start Vote Immediately
Then I will not be able to set a future time for the start of the vote

As a registered user who is currently logged in
If I navigate to the Create a Vote page and I set the option to Start Vote at a Future Time
Then I will be able to set a future time for the start of the vote

As a registered user who is currently logged in
If I navigate to the Create a Vote page and I set the option to Start Vote at a Future Time
Then I will not be able to set a time in the past for the start of the vote

As a registered user who is currently logged in
If I navigate to the Create a Vote page and I create a vote that is set to start at a future time
Then the app will send me an email once the vote time has begun, which will include the vote title, description and access code

## Assumptions/Preconditions
There is at least one registered user in the database

## Dependencies
None - all necessary dependencies have already been completed

## Effort Points
4

## Owner
Vince Nowell

## Git Feature Branch
VA-86-create-delayed-vote

## Modeling and Other Documents

Create A Vote mockups: 

![Start Immediate Vote Mockup](https://github.com/vincenowell4/KSV-Software/blob/dev/docs/ProjectManagement/UserStories/VA86-StartVoteImmediatelyMockup.png)

![Start Future Vote Mockup](https://github.com/vincenowell4/KSV-Software/blob/dev/docs/ProjectManagement/UserStories/VA86-StartVoteFutureMockup.png)


## Tasks
1. Create new datetime field in CreatedVote table: VoteStartDateTime
2. Create new boolean field in CreatedVote table: DelayedVote
3. Create new method in CreatedVoteRepository: GetDelayedVotes, which takes no parameters (it will return all rows in the CreatedVote table where DelayedVote is true)
4. Create new method in CreatedVoteRepository: GetVotesByStartDate, which takes a datetime as a parameter
5. Create new method in CreatedVoteRepository: SetDelayedVote, which takes a VoteId and datetime as parameters
6. Modify the Create method of the CreationService to not generate a VoteAccessCode for a vote that starts at a future time
7. Create a new asynchronous process that continually checks the CreatedVote table for votes where DelayedVote is true and VoteAcessCode is null
   This process should check each row that matches those criteria for the VoteStartDate time, and if the current time is equal or greater, 
   then generate a VoteAccessCode and update the row in the CreatedVote table with the generated access code
8. This same process should also generate an email to the creator of the vote, reminding them that votes can now be submitted, and including the 
   vote access code, which can be given to those who will be voting. The email should include the Vote Title and Vote Description, 
   and the Vote close date, if one was entered, or default to say that voting for the issue will be open for 24 hours from the vote start time