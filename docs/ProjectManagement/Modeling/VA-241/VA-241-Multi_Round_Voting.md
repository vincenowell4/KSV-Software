# VA-241

## Title

*As a logged-in user I want to be able to set the duration time when I create a multi-round vote, so I can control how long each round of voting will be open*

## Description

This user story is about adding the capability of setting the duration time for multi-round votes, so that when a multi-round vote is created, the vote creator can set the number of days, hours and minutes that each round will last.

We will want to add three more fields to the Create A Vote page, so that when "Multiple Choice Multi-Round Vote" is selected as the vote type, the user will see a label “Multi-Round Voting - Time Length for Each Round” and three input boxes for Days, Hours and Minutes.

By default, set the Hours field to 24, so if the user does not modify the fields, each round of a multi-round vote will last 24 hours.

### Details:

1. Create A Vote Page will have a three new fields, Days, Hours and Minutes, which will only appear when the selected 
   Voting Type is Multiple Choice Multi-Round Vote.
2. If the user selects the Multi-Round Vote option, then the three new fields will appear, and hours will default to 24.
3. The user can select 0 to 60 for minutes, 0 to 24 for hours, and 0 to 999 for days. Not all three fields can be zero;
   this will cause a validation error and let the user know that they must enter a non-zero time for at least one of the fields.
4. When a multi-round vote is submitted, the Days, Hours and Minutes fields will be used to set the duration for each round
   (after the first round, which is set by the Vote Closing Date) of the multi-round vote.

See the UI mockup in Modeling and Other Documents, below.

## Acceptance Criteria
As a user who is logged in, if I navigate to the Create a Vote page and I click on the Vote Types field and choose "Multi-Round Vote"
Then I will see fields for Multi-Round Vote duration in Days, Hours and Minutes, with the Hours field set to 24 by default

As a logged in user if I create a multi-round vote
Then I will not be allowed to submit a vote with Days, Hours and Minutes all set to zero

As a logged in user if I create a multi-round vote, then I will be able to set each round after the first to a range of days from 0 to 999,
hours from 0 to 24, and minutes from 0 to 60


## Assumptions/Preconditions
There is at least one registered user in the database

## Dependencies
None - all necessary dependencies have already been completed

## Effort Points
4

## Owner
Vince Nowell

## Git Feature Branch
f_va-241-multi-round-voting-round-duration

## Modeling and Other Documents

Mockup and class diagram: 

![Multi Round Vote Mockup](https://github.com/vincenowell4/KSV-Software/blob/f_va-241-multi-round-vote-duration/docs/ProjectManagement/Modeling/VA-241/VA-241-UI-Mockup.png)
![Class diagram](https://github.com/vincenowell4/KSV-Software/blob/f_va-241-multi-round-vote-duration/docs/ProjectManagement/Modeling/VA-241/VotingAppClassDiagram.drawio.svg)



## Tasks
1. Create three new integer fields in CreatedVote table: RoundDays, RoundHours and RoundMinutes to indicate the duration for rounds of a multi-round vote after the first round
2. Modify the Create a Vote page to add the three new fields, which should be displayed only if the voting type is multi-round voting
3. Create new method in CreatedVoteRepository: GetMultiRoundVoteDuration, which takes the VoteId as a parameter, and returns the days, hours and minutes as a comma-separated string
4. Modify the endpoint in the ApiController that creates the next round of voting, to use the RoundDays, RoundHours and RoundMinutes fields, to set the next round of voting's close date/time
