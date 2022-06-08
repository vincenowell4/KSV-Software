ALTER TABLE [CreatedVote] DROP CONSTRAINT [Fk_Created_Vote_User_ID];
ALTER TABLE [CreatedVote] DROP CONSTRAINT [Fk_Vote_Type_ID];
ALTER TABLE [VoteOptions] DROP CONSTRAINT [Fk_Options_Created_Vote_ID];
ALTER TABLE [SubmittedVote] DROP CONSTRAINT [Fk_Created_Vote_ID];
ALTER TABLE [SubmittedVote] DROP CONSTRAINT [Fk_Submitted_Vote_User_ID];
ALTER TABLE [VoteAuthorizedUsers] DROP CONSTRAINT [Fk_AuthorizedUsers_Created_Vote_ID];
ALTER TABLE [CreatedVote] DROP CONSTRAINT [Fk_TimeZone_ID];

DROP TABLE [VoteTimeZones];
DROP TABLE [CreatedVote];
DROP TABLE [SubmittedVote];
DROP TABLE [VoteOptions];
DROP TABLE [VotingUser];
DROP TABLE [VoteType];
DROP TABLE [VoteAuthorizedUsers];
DROP TABLE [AppLogs];