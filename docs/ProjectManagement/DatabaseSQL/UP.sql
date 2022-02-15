CREATE TABLE [CreatedVote] 
(
[ID] int PRIMARY KEY IDENTITY(1, 1),
[UserID] int, 
[VoteDiscription] nvarchar(1000) NOT NULL,
[Anonymous] BIT NOT NULL,
[VoteTypeId] int NOT NULL
);

CREATE TABLE [VoteType]
(
    [ID] int PRIMARY KEY IDENTITY(1, 1),
    [Type] nvarchar(500) NOT NULL, 
    [VoteTypeDescription] nvarchar(500) NOT NULL
);

CREATE TABLE [Options] 
(
[ID] int PRIMARY KEY IDENTITY(1, 1),
[CreatedVoteID] int NOT NULL,
[VoteOptionString] nvarchar(250) NOT NULL
);

CREATE TABLE [SubmittedVote] 
(
[ID] int PRIMARY KEY IDENTITY(1, 1),
[CreatedVoteID] int NOT NULL,
[VoteChoice] int NOT NULL,
[UserID] int,
[Validated] BIT NOT NULL
);

CREATE TABLE [User] 
(
[ID] int PRIMARY KEY IDENTITY(1, 1),
[Name] nvarchar(250) NOT NULL
);

ALTER TABLE [Options] ADD CONSTRAINT [Fk_Options_Created_Vote_ID]
 FOREIGN KEY ([CreatedVoteID]) REFERENCES [CreatedVote] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [SubmittedVote] ADD CONSTRAINT [Fk_Created_Vote_ID]
 FOREIGN KEY ([CreatedVoteID]) REFERENCES [CreatedVote] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [SubmittedVote] ADD CONSTRAINT [Fk_Submitted_Vote_User_ID]
 FOREIGN KEY ([UserID]) REFERENCES [User] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [CreatedVote] ADD CONSTRAINT [Fk_Created_Vote_User_ID]
 FOREIGN KEY ([UserID]) REFERENCES [User] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [CreatedVote] ADD CONSTRAINT [Fk_Vote_Type_ID]
 FOREIGN KEY ([VoteTypeId]) REFERENCES [VoteType] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;