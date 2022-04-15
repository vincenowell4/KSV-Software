Feature: VA109_Kaitlin
**As a user that is not logged in, I want to be able to see the voting results for my created vote, so that I am able to make decisions based on these results.**

This feature is about someone not being logged in being able to see a created votes results if they have the access vote (also added in that only authorized users if the
vote is private are able to see the results). If they enter a valid access code they will be brought to the Results page where they will see that vote title, description,
and a table with the voting options with totals per option, the winning option (or a tie), and the overall total votes as well. 


@tag1
Scenario: User able to see the input box for vote results on Access Page
	Given I am on the 'Access' a vote page 
	Then I will see an input box for entering an access code 
    And I will see another input box for entering an access code 

Scenario: User is brought back to access page from vote results page after clicking button 
	Given I have entered the '<AccessCode>' on the 'Access' page
	When I am on the 'Results' page 
    And I click on the back to access a vote button 
    Then I am brought back to the access a vote page 
	Examples: 
	| AccessCode |
	| 1450f7     |

Scenario: User enters a valid access code and is brought to results page 
	Given I have entered the '<AccessCode>' on the 'Access' page
	Then I am brought to the correct results page
	Examples: 
	| AccessCode |
	| 1450f7     |

Scenario: User enters invalid access code and is still on access page 
	Given I am on the 'Access' a vote page 
	And I have entered the incorrect '<AccessCode>' to access vote results
	Then I will still be on the 'Access' page 
	Examples: 
	| AccessCode |
	| 123456     |

	

#Scenario: User is redirected to submit a vote page after entering valid access code 
#    Given I am on the 'Access' a vote page
#    And I enter a valid access code for the subit a vote box
#    When I click the submit button for submitting a vote
#    Then I will be redirected to the 'SubmitVote' a vote page 
#
#Scenario: A user enters an invalid access code 
#    Given I am on the 'Access' a vote page
#    And I have entered an invalid vote access code
#    Then I will be told the code is invalid
#    And I will still be on the 'Access' a vote page 
#
#Scenario: A user sees the correct vote title and description for the vote they clicked 
#    Given I am on the 'Results' vote page 
#    Then I will see the correct vote title 

#Scenario: A user sees the correct vote title and description for the vote they clicked 
#    Given I am on the 'Results' vote page 
#    Then I will see the correct vote description 

