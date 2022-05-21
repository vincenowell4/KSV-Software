Feature: VA242

**As a user who is not logged in, I want to see a CAPTCHA widget on pages that ask me for input, so that I can prove that I am a human and not a bot**

This user story is for adding reCAPTCHA to pages on the Opiniony voting app that allow user input before a user is logged in

	
Scenario: Not logged-in user is able to see a reCAPTCHA widget
	Given I click on the Login link
    Then I will see the Login page with a reCAPTCHA widget
