# VA-82
<hr>

## Title

*As a user I want to have the description, title and vote options read out to me by the page, so I dont have to read the entire vote.
*

## Description

For this story we want the user to be able to listen to the vote title, description, and options for that vote. We want the user to be able to play pause and rewind the audio for the page. This option would be available on the submit a vote page for users that go to cast a vote and want the convenience of listening to the vote rather than reading it. 

The audio should be dynamic to the details of the vote and votes with multiple options should have all options read off, if there are special details about the vote like the type it should be read off as well.

### Details:
1. If the user in on the submit a vote page, there should be an options to listen to the vote.
2. The user should be able to play/pause/rewind the audio
3. Audio should be dynamic to the vote

See mockup below

## Acceptance Criteria
    Given I am on the submit a vote page
    Then I should see an options to play an audio version of the vote
    
    Given I am on the submit a vote page
    And I have clicked the audio button 
    Then I should have the option to pause and rewind the audio

    Given I am on the Vote Review page
    Then I should see an options to play an audio version of the each vote
    
    Given I am on the Vote Review page
    And I have clicked the audio button 
    Then I should have the option to pause and rewind the audio

    Given I am on the Vote History Page
    Then I should see an options to play an audio version of each of the votes
    
    Given I am on the Vote History Page
    And I have clicked the audio button 
    Then I should have the option to pause and rewind the audio




## Assumptions/Preconditions
Database is connected, lazy loading is being used by the app. User login is working and can be used in the controller and page.

## Dependencies
None

## Effort Points
4
## Owner
Sam Torris
## Git Feature Branch
f_add_googleTTS_to_pages_VA_82

## Modeling and Other Documents


## Tasks
1. research google tts api
2. build methods to use google tts api
3. retrive vote data from the db and parse into single string value
4. send data to google api and retrieve
5. research how to send audio to the html page
6. add audio controls to each page
7. link audio to each vote on the given pages