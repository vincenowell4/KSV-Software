namespace VotingApp.Data
{
    public static class AppStrings
    {
        public const string EmailSubject = "Opiniony Confirmation";
        public const string EmailMessage = "Welcome to Opiniony! Please confirm your account registration by";
        public const string EmailSentMessage = "Verification email sent. Please check your email";
        public const string EmailConfirmationSuccessful = "Thank you for confirming your email";
        public const string EmailConfirmationError = "There was an error confirming your email. To resend the email, please click";
        public const string EmailLoadUserError = "Unable to load user with email";
        public const string EmailSubjectVoteOpen = "Opiniony - Your Vote Is Now Open";
        public const string EmailMessageVoteOpen = "The vote you created is now open: ";
        public const string EmailSubjectMultiRoundVoteResults = "Opiniony - Your Multi-Round Vote Results";
        public const string EmailSubjectMultiRoundNewVoteOpen = "Opiniony - The Next Round in your Multi-Round Vote Is Open";
        public const string EmailMessageWinningVote = "The multi-round vote you created has a winning option: ";
        public const string EmailMessageTieVote = "The multi-round vote you created is tied for a winner, or has a tie for last place: ";
        public const string EmailMessageZeroVotes = "The multi-round vote you created has no submitted votes: ";
        public const string EmailMessageNextRoundVoteOpen = "The next round in your multi-round vote is now open: ";
    }
}
