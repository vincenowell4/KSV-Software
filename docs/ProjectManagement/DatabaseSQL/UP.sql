CREATE TABLE [CreatedVote] 
(
[ID] int PRIMARY KEY IDENTITY(1, 1),
[UserID] int, 
[VoteTitle] nvarchar(350) NOT NULL,
[VoteDiscription] nvarchar(1000) NOT NULL,
[AnonymousVote] BIT NOT NULL,
[VoteTypeId] int NOT NULL,
[VoteAccessCode] NVARCHAR (100),
[VoteOpenDateTime] DATETIME,
[VoteCloseDateTime] DATETIME,
[PrivateVote] BIT NOT NULL,
[VoteAudioBytes] VARBINARY(max),
[RoundNumber] int NOT NULL,
[NextRoundId] int NOT NULL
);

CREATE TABLE [VoteType]
(
    [ID] int PRIMARY KEY IDENTITY(1, 1),
    [VotingType] nvarchar(500) NOT NULL, 
    [VoteTypeDescription] nvarchar(500) NOT NULL
);

CREATE TABLE [VoteOptions] 
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
[Validated] BIT NOT NULL,
[DateCast] DATETIME
);

CREATE TABLE [VotingUser] 
(
[ID] int PRIMARY KEY IDENTITY(1, 1),
[NetUserID] NVARCHAR(450) NOT NULL,
[UserName] nvarchar(250) NOT NULL
);

CREATE TABLE [VoteAuthorizedUsers] 
(
[ID] int PRIMARY KEY IDENTITY(1, 1),
[CreatedVoteID] int NOT NULL,
[UserName] nvarchar(250) NOT NULL
);

CREATE TABLE [AppLogs]
(
[ID] int PRIMARY KEY IDENTITY(1, 1),
[Date] DATETIME NOT NULL,
[LogLevel] nvarchar(10) NOT NULL,
[LogMessage] nvarchar(500) NOT NULL   
);


ALTER TABLE [VoteOptions] ADD CONSTRAINT [Fk_Options_Created_Vote_ID]
 FOREIGN KEY ([CreatedVoteID]) REFERENCES [CreatedVote] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [SubmittedVote] ADD CONSTRAINT [Fk_Created_Vote_ID]
 FOREIGN KEY ([CreatedVoteID]) REFERENCES [CreatedVote] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [SubmittedVote] ADD CONSTRAINT [Fk_Submitted_Vote_User_ID]
 FOREIGN KEY ([UserID]) REFERENCES [VotingUser] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [CreatedVote] ADD CONSTRAINT [Fk_Created_Vote_User_ID]
 FOREIGN KEY ([UserID]) REFERENCES [VotingUser] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
ALTER TABLE [CreatedVote] ADD CONSTRAINT [Fk_Vote_Type_ID]
 FOREIGN KEY ([VoteTypeId]) REFERENCES [VoteType] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;
 ALTER TABLE [VoteAuthorizedUsers] ADD CONSTRAINT [Fk_AuthorizedUsers_Created_Vote_ID]
 FOREIGN KEY ([CreatedVoteID]) REFERENCES [CreatedVote] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;