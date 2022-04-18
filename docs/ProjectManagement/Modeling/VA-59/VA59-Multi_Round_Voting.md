# VA-59

## Title

*As a user I want to be able to create multi-round votes, so voters can narrow down options*

## Description

This user story is about adding the capability of creating multi-round votes that will allow the number of vote options to be narrowed down with each round of voting. This may help voters decide on a final option, when there are many to begin with.

We will want to add more vote type options to the Create A Vote page. Currently there are two options: Yes/No and Multiple Choice. An example of a multi-round option could be "Multiple Choice Multi-Round" (it wouldn't make sense to have multi-round voting forn Yes/No votes).

If a vote creator chooses "Multiple Choice Multi-Round", then the minimum number of vote options would be three.

### Details:

1. Create A Vote Page will have a new vote type option, Multpile Choice Multi-Round.
2. If the user selects the Multi-round option, then the minimum number of options when creating the vote will be three.
3. For multi-round votes, once the vote close date/time for round 1 has passed the app will create another vote for the second round, and email the vote creator with the Vote Access Code and URL to the Submit Vote page.
4. Step 3 will be repeated until a vote option has a majority, or there is a tie between available options.
5. In the event that a round ends with a tie for each option (no clear winner, no clear option to remove), then voting should stop, and the vote creator notified of the status of the vote.

See the UI mockup in Modeling and Other Documents, below.

## Acceptance Criteria
As a registered user if I navigate to the Create a Vote page and I click on the Vote Types field
Then I will see, in addition to Yes/No and Multiple Choice, another option "Multi-Round Multiple Choice"

As a registered user if I create a multi-round vote
Then on the vote options page, I will need to enter at least three vote options

As a registered user if I create a multi-round vote, and the first round of voting is closed
Then I should get an email telling me that the next round of voting is now open, with an access code and details of the new vote

As a registered user if I create a multi-round vote, and the Voting App determines one of the vote options receoved a majority of the votes
Then I should get an email notifiying me of the final results of the multi-round vote

As a registered user if I create a multi-round vote, and the Voting App determines that there is a tie
Then I should get an email notifiying me that the last round of voting led to a tie


## Assumptions/Preconditions
There is at least one registered user in the database

## Dependencies
None - all necessary dependencies have already been completed

## Effort Points
8

## Owner
Vince Nowell

## Git Feature Branch
f_va-59-multi-round-voting

## Modeling and Other Documents

Mockup and class diagram: 

![Multi Round Vote Mockup](https://github.com/vincenowell4/KSV-Software/blob/f_va-59-multi-round-voting/docs/ProjectManagement/Modeling/VA-59/VA-59-UI-Mockup.png)
![Class diagram](https://github.com/vincenowell4/KSV-Software/blob/f_va-59-multi-round-voting/docs/ProjectManagement/Modeling/VA-59/VotingAppClassDiagarm.drawio.svg)



## Tasks
1. Create new bit field in CreatedVote table: RoundOver to indicate that this round of a multi-round vote is over
2. Create new table: VoteRounds, so history of multi-round-voting can be tracked
3. Add an entry to SEED.Sql for the new multi-round vote option
4. Modify the Create a Vote page with a new card to explain multi-round voting
5. Modify the current repo method for creating a vote to also set the new RoundOver flag to the appropriate value
6. Create new method in CreatedVoteRepository: GetClosedMutliRoundVotes, which takes no parameters 
   (it will return all rows in the CreatedVote table where VotingType is 3, CloseDate is in the past, and RoundOver is not true)
7. Create new method in CreatedVoteRepository: GetVoteRoundsHistory, which takes a vote ID and returns the history of multi-round votes
8. Create new method in CreatedVoteRepository: AddToVoteRoundsHistory, which takes an old vote ID, new vote ID, and the current round
   and adds a new row to VoteRounds
9. Create new endpoints in the ApiController to get multi-round CreatedVotes, to get vote options for votes, to create a new vote,
   to add options to a vote, to check the VoteRounds table for multi-round vote history, and to add a new row to VoteRounds table.
10. Modify the VotingApp service to check for multi-round votes, apply the algorithm for creating the next round of voting, and creating the
    next round of voting.   
11. This same process should also generate an email to the creator of the vote, with the access code for the next round of voting, plus details
    of the next round of voting
