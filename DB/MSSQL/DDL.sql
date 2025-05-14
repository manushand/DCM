create table [Game]
(
    [id]              INTEGER identity(1,1)
        primary key,
    [roundId]         INTEGER not null,
    [number]          INTEGER not null default 1,
    [status]          INTEGER not null default 0,
    [scoringSystemId] INTEGER,
    [date]            TIMESTAMP,
    [name]            VARCHAR(80)
);

create index [Game_RoundId]
    on [Game] ([roundId]);

create index [Game_Number]
    on [Game] ([number]);

create index [Game_ScoringSystemId]
    on [Game] ([scoringSystemId]);

CREATE index [UI_Game_Number_RoundId]
    ON [Game] ([number], [roundId])

create table [Player]
(
    [id]           INTEGER identity(1,1)
        primary key,
    [firstName]    VARCHAR(50) not null,
    [lastName]     VARCHAR(50) not null,
    [emailAddress] VARCHAR(80)
);

create table [GamePlayer]
(
    [id]       INTEGER IDENTITY(1,1) -- do not reference in model or code; does not exist in Access -- for performance only
         primary key,
    [gameId]   INTEGER not null,
    [playerId] INTEGER not null,
    [power]    INTEGER not null default 0,
    [result]   INTEGER not null default 0,
    [years]    INTEGER,
    [centers]  INTEGER,
    [other]    DECIMAL not null default 0,
);

create unique index [GamePlayer_GameId_PlayerId]
    on [GamePlayer] ([gameId], [playerId])

create index [GamePlayer_GameId]
    on [GamePlayer] ([gameId]);

create index [GamePlayer_PlayerId]
    on [GamePlayer] ([playerId]);

create index [GamePlayer_Power]
    on [GamePlayer] ([power]);

create table [Group]
(
    [id]          INTEGER identity(1,1)
        primary key,
    [name]        VARCHAR(50) not null,
    [conflict]    INTEGER not null default 0,
    [description] VARCHAR(MAX)
);

create table [GroupPlayer]
(
    [id] INT IDENTITY(1,1) -- do not reference in model or code; does not exist in Access -- column for performance only
        primary key,
    [playerId] INTEGER not null,
    [groupId]  INTEGER not null
);

create unique index [GroupPlayer_GroupId_PlayerId]
    on [GroupPlayer] ([groupId], [playerId]);

create index [GroupPlayer_GroupId]
    on [GroupPlayer] ([groupId]);

create index [GroupPlayer_PlayerId]
    on [GroupPlayer] ([playerId]);

create table [PlayerConflict]
(
    [id]      INTEGER IDENTITY(1,1) -- do not reference in model or code; does not exist in Access -- column for performance only
        primary key,
    [player1] INTEGER not null,
    [player2] INTEGER not null,
    [value]   INTEGER not null default 0
);

CREATE UNIQUE index [UI_PlayerConflict_Player1_Player2]
    ON [PlayerConflict] ([player1], [player2])

create table [Round]
(
    [id]              INTEGER identity(1,1)
        primary key,
    [tournamentId]    INTEGER not null,
    [number]          INTEGER not null,
    [scoringSystemId] INTEGER,
    constraint Round_TournamentId_Number
        unique ([number], [tournamentId])
);

create index [Round_Number]
    on [Round] ([number]);

create index [Round_ScoringSystemId]
    on [Round] ([scoringSystemId]);

create index [Round_TournamentId]
    on [Round] ([tournamentId]);

create table [RoundPlayer]
(
    [id]       INTEGER IDENTITY(1,1) -- do not reference in model or code; does not exist in Access -- column for performance only
        primary key,
    [playerId] INTEGER not null,
    [roundId]  INTEGER not null
);

create unique index [RoundPlayer_RoundId_PlayerId]
    on [RoundPlayer] ([roundId], [playerId]);

create index [RoundPlayer_RoundId]
    on [RoundPlayer] ([roundId]);

create index [RoundPlayer_PlayerId]
    on [RoundPlayer] ([playerId]);

create table [ScoringSystem]
(
    [id]                      INTEGER identity(1,1)
        primary key,
    [name]                    VARCHAR(50)
        constraint ScoringSystem_Name
            unique,
    [usesGameResult]          BIT not null default 0,
    [usesCenterCount]         BIT not null default 0,
    [usesYearsPlayed]         BIT not null default 0,
    [finalGameYear]           INTEGER,
    [playerAnteFormula]       VARCHAR(MAX),
    [provisionalScoreFormula] VARCHAR(MAX),
    [finalScoreFormula]       VARCHAR(MAX) not null,
    [testGameData]            VARCHAR(150) not null,
    [significantDigits]       INTEGER not null default 0,
    [drawPermissions]         INTEGER not null default 0,
    [pointsPerGame]           INTEGER,
    [otherScoreAlias]         VARCHAR(50)
);

create table [Team]
(
    [id]           INTEGER identity(1,1)
        primary key,
    [tournamentId] INTEGER not null,
    [name]         VARCHAR(50)
);

create index [Team_TournamentId]
    on [Team] ([tournamentId]);

create table [TeamPlayer]
(
    [id]       INTEGER IDENTITY(1,1) -- do not reference in model or code; does not exist in Access -- column for performance only
        primary key,
    [playerId] INTEGER not null,
    [teamId]   INTEGER not null
);

create unique index [TeamPlayer_TeamId_PlayerId]
    on [TeamPlayer] ([teamId], [playerId]);

create index [TeamPlayer_PlayerId]
    on [TeamPlayer] ([playerId]);

create index [TeamPlayer_TeamId]
    on [TeamPlayer] ([teamId]);

create table [Tournament]
(
    [id]              INTEGER identity(1,1)
        primary key,
    [name]            VARCHAR(50) not null
        constraint Tournament_Name
            unique,
    [description]     VARCHAR(MAX),
    [scoringSystemId] INTEGER   not null,
    [teamSize]        INTEGER   not null default 0,
    [teamRound]       INTEGER   not null default 0,
    [teamConflict]    INTEGER   not null default 0,
    [playerConflict]  INTEGER   not null default 0,
    [powerConflict]   INTEGER   not null default 0,
    [totalRounds]     INTEGER   not null default 0,
    [roundsToDrop]    INTEGER   not null default 0,
    [minimumRounds]   INTEGER   not null default 0,
    [groupPowers]     INTEGER   not null default 0,
    [unplayedScore]   INTEGER   not null default 0,
    [scalePercentage] DECIMAL   not null default 0,
    [assignPowers]    BIT       not null default 0,
    [scoreConflict]   INTEGER   not null default 0,
    [groupId]         INTEGER,
    [date]            TIMESTAMP
);

create index [Tournament_Id]
    on [Tournament] ([id]);

create index [Tournament_TotalRounds]
    on [Tournament] ([totalRounds]);

create index [Tournament_ScoringSystemId]
    on [Tournament] ([scoringSystemId]);

create table [TournamentPlayer]
(
    [id]           INTEGER IDENTITY(1,1) -- do not reference in model or code; does not exist in Access -- for performance only
        primary key,
    [tournamentId] INTEGER not null,
    [playerId]     INTEGER not null,
    [roundNumbers] INTEGER not null default 0
);

create unique index [TournamentPlayer_TournamentId_PlayerId]
    on [TournamentPlayer] ([tournamentId], [playerId]);

create index [TournamentPlayer_PlayerId]
    on [TournamentPlayer] ([playerId]);

create index [TournamentPlayer_TournamentId]
    on [TournamentPlayer] ([tournamentId]);

-- FOREIGN KEYS --

ALTER TABLE [GroupPlayer] WITH CHECK
    ADD CONSTRAINT [FK_GroupPlayer_PlayerId]
        FOREIGN KEY([PlayerId])
        REFERENCES [Player] ([Id]);

ALTER TABLE [GroupPlayer] WITH CHECK
    ADD CONSTRAINT [FK_GroupPlayer_GroupId]
        FOREIGN KEY([GroupId])
        REFERENCES [Group] ([Id]);

ALTER TABLE [GamePlayer] WITH CHECK
    ADD CONSTRAINT [FK_GamePlayer_GameId]
        FOREIGN KEY([GameId])
        REFERENCES [Game] ([Id]);

ALTER TABLE [GamePlayer] WITH CHECK
    ADD CONSTRAINT [FK_GamePlayer_PlayerId]
        FOREIGN KEY([PlayerId])
        REFERENCES [Player] ([Id]);

ALTER TABLE [RoundPlayer] WITH CHECK
    ADD CONSTRAINT [FK_RoundPlayer_RoundId]
        FOREIGN KEY([RoundId])
        REFERENCES [Round] ([Id]);

ALTER TABLE [RoundPlayer] WITH CHECK
    ADD CONSTRAINT [FK_RoundPlayer_PlayerId]
        FOREIGN KEY([PlayerId])
        REFERENCES [Player] ([Id]);

ALTER TABLE [TeamPlayer] WITH CHECK
    ADD CONSTRAINT [FK_RoundPlayer_TeamId]
        FOREIGN KEY([TeamId])
        REFERENCES [Round] ([Id]);

ALTER TABLE [TeamPlayer] WITH CHECK
    ADD CONSTRAINT [FK_TeamPlayer_PlayerId]
        FOREIGN KEY([PlayerId])
        REFERENCES [Player] ([Id]);

ALTER TABLE [PlayerConflict] WITH CHECK
    ADD CONSTRAINT [FK_GamePlayer_Player1]
        FOREIGN KEY([Player1])
        REFERENCES [Player] ([Id]);

ALTER TABLE [PlayerConflict] WITH CHECK
    ADD CONSTRAINT [FK_GamePlayer_Player2]
        FOREIGN KEY([Player2])
        REFERENCES [Player] ([Id]);

ALTER TABLE [Team] WITH CHECK
    ADD CONSTRAINT [FK_Team_TournamentId]
        FOREIGN KEY([TournamentId])
        REFERENCES [Tournament] ([Id]);

-- All Tournaments must have a ScoringSystem. Non-competitive Groups simply do not have a Host Tournament.
ALTER TABLE [Tournament] WITH CHECK
    ADD CONSTRAINT [FK_TournamentId_ScoringSystemId]
        FOREIGN KEY([ScoringSystemId])
        REFERENCES [ScoringSystem] ([Id]);

-- All Games have a RoundId, even if that is the Host Round in a Host Tournament for Group games
ALTER TABLE [Game] WITH CHECK
    ADD CONSTRAINT [FK_Game_RoundId]
        FOREIGN KEY([RoundId])
        REFERENCES [Round] ([Id]);

-- All Rounds have a TournamentId even if it's a Host Tournament for games played by a Group
ALTER TABLE [Round] WITH CHECK
    ADD CONSTRAINT [FK_Round_TournamentId]
        FOREIGN KEY([TournamentId])
        REFERENCES [Tournament] ([Id]);

-- The foreign keys below are for columns that may be NULL

-- A Tournament with a null GroupId is an independent event, not a host Tournament for a Group
ALTER TABLE [Tournament] WITH CHECK
    ADD CONSTRAINT [FK_Tournament_GroupId]
        FOREIGN KEY([GroupId])
            REFERENCES [Group] ([Id]);

-- A Game with a null scoring system will use its Round.ScoringSystem
ALTER TABLE [Game] WITH CHECK
    ADD CONSTRAINT [FK_Game_ScoringSystemId]
        FOREIGN KEY([ScoringSystemId])
        REFERENCES [ScoringSystem] ([Id]);

-- A Round with a null scoring system will use its Tournament.ScoringSystem
ALTER TABLE [Round] WITH CHECK
    ADD CONSTRAINT [FK_Round_ScoringSystemId]
        FOREIGN KEY([ScoringSystemId])
        REFERENCES [ScoringSystem] ([Id]);

-- CHECK CONSTRAINTS --

-- Players cannot conflict with themselves
ALTER TABLE [PlayerConflict]
    ADD CONSTRAINT [CHK_PlayerConflict_Player1_Player2]
        CHECK (Player1 <> Player2);

-- Game numbers must be positive
ALTER TABLE [Game]
    ADD CONSTRAINT [CHK_Game_Number]
        CHECK ([Number] > 1);

-- Round numbers may be 0 (Group games host round) or 1-9 (Tournament Round)
ALTER TABLE [Round]
    ADD CONSTRAINT [CHK_Round_Number]
        CHECK ([Number] BETWEEN 0 AND 9);

-- Game.Statuses are NotStarted=0, Underway=1, Completed=2
ALTER TABLE [Game]
    ADD CONSTRAINT [CHK_Game_Status]
        CHECK ([Status] IN (0, 1, 2));

-- GamePlayer.Powers are TBD=-1, Austria=0, ..., Turkey=6
ALTER TABLE [GamePlayer]
    ADD CONSTRAINT [CHK_GamePlayer_Power]
        CHECK ([Power] BETWEEN -1 AND 6);

-- GamePlayer.Results are Unknown=-1, Loss=0, Win=1
ALTER TABLE [GamePlayer]
    ADD CONSTRAINT [CHK_GamePlayer_Result]
        CHECK ([Result] BETWEEN -1 AND 1);

-- GamePlayer.Years are null or must be 1-18
ALTER TABLE [GamePlayer]
    ADD CONSTRAINT [CHK_GamePlayer_Years]
        CHECK ([Years] IS NULL OR [Years] BETWEEN 1 AND 18);

-- GamePlayer.Centers are null or must be 0-34
ALTER TABLE [GamePlayer]
    ADD CONSTRAINT [CHK_GamePlayer_Centers]
        CHECK ([Centers] IS NULL OR [Centers] BETWEEN 0 AND 34);

-- ScoringSystem.DrawPermissions must be None=0, All=1, DIAS=2
ALTER TABLE [ScoringSystem]
    ADD CONSTRAINT [CHK_ScoringSystem_DrawPermissions]
        CHECK ([DrawPermissions] in (0, 1, 2));

-- ScoringSystem.FinalGameYear may be null (unset) or between 1907 and 1918
ALTER TABLE [ScoringSystem]
    ADD CONSTRAINT [CHK_ScoringSystem_FinalGameYear]
        CHECK ([FinalGameYear] IS NULL OR [FinalGameYear] BETWEEN 1907 AND 1918);

-- Tournament.PowerGroups must be None=0, EastWest=1, Corners=2, Naval=3, LandSea=4, FleetNear=5, Lepanto=6
ALTER TABLE [Tournament]
    ADD CONSTRAINT [CHK_Tournament_GroupPowers]
        CHECK ([GroupPowers] BETWEEN 0 AND 6);
