ALTER TABLE [CreatedVote] DROP CONSTRAINT [Fk_Created_Vote_User_ID];
ALTER TABLE [CreatedVote] DROP CONSTRAIN [Fk_Vote_Type_ID];
ALTER TABLE [Options] DROP CONSTRAINT [Fk_Options_Created_Vote_ID];
ALTER TABLE [SubmittedVote] DROP CONSTRAINT [Fk_Created_Vote_ID];
ALTER TABLE [SubmittedVote] DROP CONSTRAINT [Fk_Submitted_Vote_User_ID];

DROP TABLE [CreatedVote];
DROP TABLE [SubmittedVote];
DROP TABLE [Options];
DROP TABLE [User];
DROP TABLE [VoteType];