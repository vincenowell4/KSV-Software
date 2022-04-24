# ID VA-207
<hr>

## Title

*As a user when creating a vote and reviewing a vote I would like the steps to be easier to use and organized better so that creating and reviewing votes is an easier process.*

## Description

This user story is about organizing a few different pages that have become disorganized and confusing for a user. We want the user to have no trouble when creating a vote or reviewing voting results. The create a vote page needs to be organized since there are now a lot of questions a user has to answer (the page is looked clustered and confusing). Organize it into clear sections instead. This way a user who wants to quickly create a vote knows what is required to make a vote easily without having to read a bunch of things.

We also want to add the analytics and vote results to one vote results page. The table seems a bit wider than necessary so we would like the table to be smaller with either the other tables next to it or the pie chart analytics next to the table. 

On the vote review page will now just have an edit and vote results buttons. The vote review page needs some organizing as well. Add "..." to the vote description so the user knows there is more to the description. The table needs better organization as well on the vote review page. One table for open votes (ordered by date) and another table for closed votes (ordered by date again). Get rid of access link column on table. Make access code a url (the access link url) for the open vote table. 

On the vote history page make it more obvious to the user that they are able to click on the accordian somehow (either with a tooltip, or down arrow next to each accordian, etc). 

On the multiple choice page the buttons are confusing for the user being all the same color and spaced together so closely. Change the color of buttons or location so it's not in a triangle. Also change the add button to say "Add voting option" so it is clearer what add button is for. 

On the cast vote page align the radio buttons for the vote options. Also, after submitting vote align list bullets for vote options again.

### Details:
1. Organize create a vote into 3 sections : required, times(optional), and privacy (optional)
2. Add modal or collapsible next to vote type of all the vote descriptions to take up less space on page 
3. On required section of create a vote page include title, description, vote type 
4. On times section of create a vote page include start vote immediately check box, start vote later time checkbox (with time input), and vote close input box
5. On privacy section of create a vote page include vote private checkbox and vote anonymous checkbox
6. Create one vote results page that will include the pie graph and the vote results tables. Make width of columns in vote results table smaller as well. Get rid of analytics button on vote review page.
7. Add "..." next to vote descriptions on vote review page 
8. Add one table for open votes -organized by descending open date 
9. Add another table for closed votes -organized by descending close date 
10. on vote review page get rid of access link column. Instead make access code a url to that link (only for open vote table)
11. On vote history page add either a triangle symbol or something to let the user know to click on each accordian for more info
12. Organize buttons on multiple choice page so they are not all right next to each other 
13. Try to align radio buttons on casting vote process 

## Acceptance Criteria
No .feature file for this one

    Given I am on the vote review page
    And I click vote results button
    Then I should be brought to vote results page
    And I should see vote results table
    And I should see vote analytics pie graph 

    Given I am on the vote results page
    Then I should see the vote table width smaller
    And the analytics pie graph next to it 

    Given I am on the vote review page 
    Then I should see "..." next to the vote description 

    Given I am on the vote review page
    Then I should see a table for open votes

    Given I am on the vote review page
    Then I should see a table for closed votes

    Given I am on the vote review page
    And there is an open vote table
    Then I should see the votes in descending order by open vote date

    Given I am on the vote review page
    And there is a closed vote table
    Then I should see the votes in descending order by closed vote date

    Given I am on the vote review page 
    And I am looking at the open vote table
    And I click on the url under access code
    Then I will be brought to the cast a vote page for that vote

    Given I am on the vote history page
    Then I should see an icon or tool tip so that I understand to click it 

    Given I am on the multiple choice page
    Then the buttons will not all be next to each other (edit, submit, add)

    Given I am on the create a vote page 
    And I click on the modal (or collapsible) for vote types
    Then I will see descriptions for each vote type

    Given I am on the create a vote page
    Then I will see one area clearly labeled required
    And it will contain a vote title
    And a vote description
    And a vote type 

    Given I am on the create a vote page
    Then I will see an area labeled private (optional)

    Given I am on the create a vote page
    Then I will see an area labeled times (optional)


## Assumptions/Preconditions
None
 
## Dependencies
None (no other user stories in this sprint) 

## Effort Points
4

## Owner
Kaitlin

## Git Feature Branch
f_va207_organize_pages

## Modeling and Other Documents
See Create A Vote page UI MockUp under Modeling folder 
See Vote Review page UI Mockup under Modeling folder 
See Vote Results page UI mockup under Modeling folder 

## Tasks
1. ...