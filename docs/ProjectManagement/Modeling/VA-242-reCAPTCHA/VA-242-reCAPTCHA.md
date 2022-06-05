# VA-242


## Title

*As a user who is not logged in, I want to see a CAPTCHA widget on pages that ask me for input, so that I can prove that I am a human and not a bot*


## Description

This user story is for adding reCAPTCHA to pages on the Opiniony voting app that allow user input before a user is logged in


### Details:

1. Register the site with Google, and obtain site and secret keys (store the secret key in the secrets file)
2. Add the reCAPTCHA script tag to _Layout.cshtml
3. Add the reCAPTCHA widget to each page in the voting app that allows user input before the user is logged in

See the UI mockup in Modeling and Other Documents, below.


## Acceptance Criteria
As a user, if I navigate to a page that allows user input before being logged in, like the Log In page,
Then I will see a reCAPTCHA widget on that page

As a user, if I attempt to submit data on a page before checking the checkbox in the reCAPTCHA widget, 
Then I will get an error message asking me to check the box in the reCAPTCHA widget

As a user, if I attempt to submit data on a page after checking the checkbox in the reCAPTCHA widget, 
Then I will be allowed to proceed to the next page


## Assumptions/Preconditions
None


## Dependencies
None


## Effort Points
2


## Owner
Vince Nowell


## Git Feature Branch
f_recaptcha


## Modeling and Other Documents

Mockup: 

![Recaptcha Mockup](https://github.com/vincenowell4/KSV-Software/blob/f_recaptcha/docs/ProjectManagement/Modeling/VA-242/VA-242-UI-Mockup.png)


## Tasks
1. Register the site with Google, get site/secret keys (store the secret key in the secrets file), and add script tag to _Layout.cshtml
2. Add the reCAPTCHA widget to each page in the voting app that allows user input before the user is logged in
