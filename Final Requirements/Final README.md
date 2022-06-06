# Configuration Keys

1. **EmailUserName** - email address for the sendinblue account - this email is the one used to sign into the sendinblue account is what allows the app to send emails to users. This field will be the email address for the account.

2. **EmailPassword** - password for the sendinblue account - This is the password used to sign into the sendinblue account.  Like the previous mentioned this allows the app to send emails.

3. **SeedAdminPW** - The seed password for the seed admin account - This can be any string that meets Identity password requirements, this will set the password for the seed admin account

4. **GOOGLECREDS** - The credentials for the Google API - to get this value you will need to generate a key. You will need to go to the google cloud dashboard and go to apis and services, then go to credentials, then click on the service account or create one, go to keys and add key and select json. Then copy the contents of that file as a single line for this key.

5. **VotingAppApiKey** - the voting app API key is an authentication token - it is a random string of characters that is sent by the Voting App Service in the header of all API requests, and is verified by the Voting App in the API controller, to ensure that the API can’t be accessed without passing  this secret key.

6. **RecaptchaKey** - this is the secret key for Googles reCAPTCHA service, used in conjunction with the site key to authorize communication between the Voting App and the reCAPTCHA server, to verify the users response to the reCAPTCHA checkbox displayed on the forms in the Voting App.


