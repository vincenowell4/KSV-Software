# VA-243


## Title

*As a user who has created a multi-round vote, I would like to have a way to see the history for each round of the vote, so I can see the results for each round, which options were removed after each round, and other details*


## Description

This user story is about adding the ability to see the vote history for a multi-round poll. Currently, there is no easy way to see which option(s) were removed
after each round of voting; this story would let the user see, on one page, each round of a multiround poll, and the options for each round, plus the number of
votes for each option and who voted for each option (if the poll is not anonymous).


### Details:

1. Change the Poll Review page so that polls created as multi-round appear in a separate table from Open and Closed Polls
2. Change the Poll Results page so that for multi-round polls, it will display each round of the poll, in order

See the UI mockup in Modeling and Other Documents, below.


## Acceptance Criteria
As a user who has created multi-round polls, if I navigate to the Poll Review page,
Then I will see a Multi-Round Poll table containing the multi-round polls I have created

As a user who has created multi-round polls, if I am on the Poll Review page and I click on the Poll Results button for a multi-round poll,
Then I will be taken to the Poll Results page for that Multi-Round Poll with details for each round of the poll


## Assumptions/Preconditions
The logged in user has created one or more multi-round polls


## Dependencies
None - all necessary dependencies have already been completed


## Effort Points
4


## Owner
Vince Nowell


## Git Feature Branch
f_multi-round-history


## Modeling and Other Documents

Mockup and class diagram: 

![Multi Round Vote Mockup](https://github.com/vincenowell4/KSV-Software/blob/f_multi-round-history/docs/ProjectManagement/Modeling/VA-243/VA-243-UI-Mockup1.png)
![Multi Round Vote Mockup](https://github.com/vincenowell4/KSV-Software/blob/f_multi-round-history/docs/ProjectManagement/Modeling/VA-243/VA-243-UI-Mockup2.png)


## Tasks
1. Add a new view model, VotesResultsVM, which will be an array of the existing VoteResultVM
2. Modify the CreateController to use this new ViewModel when returning vote results
3. Modify the CreatedVotesReview page to show multi-round votes as a separate table
4. Modify the VoteResults page to show multiple rounds of voting, if displaying a multi-round poll

