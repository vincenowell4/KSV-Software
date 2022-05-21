# ID VA-268
<hr>

## Title

*As a user I want an updated help page, so that I can get help on creating a vote and understand how the voting process works amongst all the different options.*

## Description

This user story is about updating the help page for a user. Currently this page is outdated for a user if they need help creating a vote. We want this page to be organized nicely so a user can easily understand the process for creating any of the 3 voting options we provide. We need to add the directions for a multi round vote as well so that a user can understand this process as well. Update all the information to include vote close times, private vote, anonymous vote, etc. Also add a FAQ section with additional questions users may be confused about with answers to those questions. 

### Details:
1. Update any directions for other voting types
2. Organize page to look nice 
3. Add instructions for multi-round vote  
4. Add FAQS with answers, organized nicely 

## Acceptance Criteria
No .feature file for this one

    Given I am on the help page 
    Then I should see updated directions for a mult. choice vote 

    Given I am on the help page 
    Then I should see updated directions for a yes/no vote

    Given I am on the help page 
    Then I should see directions for a multi-round vote 

    Given I am on the help page 
    And I am looking at the multi-round vote 
    Then I should see a create a poll button

    Given I click on the create a poll button
    Then I should be brought to the create page 

    Given I am on the help page 
    Then I should see a FAQ title 

    Given I am on the help page 
    Then I should different cards with FAQ 

    Given I am on the help page
    And I see FAQ cards
    Then I should see answers for these FAQs

## Assumptions/Preconditions
Help page already created 
 
## Dependencies
None (no other user stories in this sprint) 

## Effort Points
4

## Owner
Kaitlin

## Git Feature Branch
f_va268_update_help_page

## Modeling and Other Documents 
Help Page UI doc in modeling folder

## Tasks
1. Change current cards to be accordians instead
2. Update directions for yes/no vote
3. Update directions for multiple choice vote 
4. Add directions for multi-round vote 
5. Create FAQ questions
6. Make cards for FAQ with answers to each question 
7. On Identity manage page make header white so user can see 
8. On Identity manage page remove phone number field (not needed)
9. Change navbar on identity page to not include 2 factor authentication tab (not needed)
10. Across all identity pages make text white, add spacing between boxes as well 
11. On multiple choice page fix tool tip to include multi-round poll 
12. Add in BDD tests