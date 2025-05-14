create table "Game"
(
    "id"              INTEGER identity
        primary key,
    "roundId"         INTEGER     default 0,
    "number"          INTEGER     default 0,
    "status"          INTEGER     default 0,
    "scoringSystemId" INTEGER     default 0,
    "date"            TIMESTAMP   default NULL,
    "name"            VARCHAR(80) default NULL,
    constraint GAME_IDXGAMENUM
        unique ("number", "roundId")
);

create index GAME_ROUNDID
    on "Game" ("roundId");

create index GAME_NUMBER
    on "Game" ("number");

create index GAME_SYSTEMID
    on "Game" ("scoringSystemId");

create table "GamePlayer"
(
    "gameId"   INTEGER default 0 not null,
    "playerId" INTEGER default 0 not null,
    "power"    INTEGER default 0,
    "result"   INTEGER default 0,
    "years"    INTEGER default 0,
    "centers"  INTEGER default 0,
    "other"    DOUBLE  default 0 not null,
    primary key ("gameId", "playerId")
);

create index GAMEPLAYER_GAMEID
    on "GamePlayer" ("gameId");

create index GAMEPLAYER_PLAYERID
    on "GamePlayer" ("playerId");

create index GAMEPLAYER_POWERID
    on "GamePlayer" ("power");

create table "Group"
(
    "id"          INTEGER identity,
    "name"        VARCHAR(50),
    "conflict"    INTEGER default 0,
    "description" VARCHAR(16777216)
);

create table "GroupPlayer"
(
    "playerID" INTEGER default 0 not null,
    "groupID"  INTEGER default 0 not null,
    primary key ("playerID", "groupID")
);

create index GROUPPLAYER_GROUPID
    on "GroupPlayer" ("groupID");

create index GROUPPLAYER_PLAYERID
    on "GroupPlayer" ("playerID");

create table "Player"
(
    "id"           INTEGER identity
        primary key,
    "firstName"    VARCHAR(50),
    "lastName"     VARCHAR(50),
    "emailAddress" VARCHAR(80)
);

create index PLAYER_ID
    on "Player" ("id");

create table "PlayerConflict"
(
    "player1" INTEGER default 0 not null,
    "player2" INTEGER default 0 not null,
    "value"   INTEGER default 0,
    primary key ("player1", "player2")
);

create table "Round"
(
    "id"              INTEGER identity
        primary key,
    "tournamentId"    INTEGER default 0,
    "number"          INTEGER default 0,
    "scoringSystemId" INTEGER default 0,
    constraint ROUND_IDXHEATNUM
        unique ("number", "tournamentId")
);

create index ROUND_NUMBER
    on "Round" ("number");

create index ROUND_SYSTEMID
    on "Round" ("scoringSystemId");

create index ROUND_TOURNAMENTID
    on "Round" ("tournamentId");

create table "RoundPlayer"
(
    "playerId" INTEGER default 0 not null,
    "roundId"  INTEGER default 0 not null,
    primary key ("roundId", "playerId")
);

create index ROUNDPLAYER_HEATID
    on "RoundPlayer" ("roundId");

create index ROUNDPLAYER_PLAYERID
    on "RoundPlayer" ("playerId");

create table "ScoringSystem"
(
    "id"                      INTEGER identity
        primary key,
    "name"                    VARCHAR(50)
        constraint SCORINGSYSTEM_IDXNAME
            unique,
    "usesGameResult"          BOOLEAN,
    "usesCenterCount"         BOOLEAN,
    "usesYearsPlayed"         BOOLEAN,
    "finalGameYear"           INTEGER,
    "provisionalScoreFormula" VARCHAR(16777216),
    "finalScoreFormula"       VARCHAR(16777216),
    "testGameData"            VARCHAR(150),
    "significantDigits"       INTEGER,
    "drawPermissions"         INTEGER,
    "pointsPerGame"           INTEGER,
    "otherScoreAlias"         VARCHAR(50) default NULL,
    "playerAnteFormula"       VARCHAR(16777216)
);

create table "Team"
(
    "id"           INTEGER identity
        primary key,
    "tournamentId" INTEGER default 0,
    "name"         VARCHAR(50)
);

create index TEAM_ID
    on "Team" ("id");

create index TEAM_TOURNAMENTID
    on "Team" ("tournamentId");

create table "TeamPlayer"
(
    "playerId" INTEGER default 0 not null,
    "teamId"   INTEGER default 0 not null,
    primary key ("playerId", "teamId")
);

create index TEAMPLAYER_PLAYERID
    on "TeamPlayer" ("playerId");

create index TEAMPLAYER_TEAMID
    on "TeamPlayer" ("teamId");

create table "Tournament"
(
    "id"              INTEGER identity
        primary key,
    "name"            VARCHAR(50)
        constraint TOURNAMENT_IDXNAME
            unique,
    "description"     VARCHAR(16777216),
    "scoringSystemId" INTEGER   default 0,
    "teamSize"        INTEGER   default 0,
    "teamRound"       INTEGER   default 0,
    "teamConflict"    INTEGER   default 0,
    "playerConflict"  INTEGER   default 0,
    "powerConflict"   INTEGER   default 0,
    "totalRounds"     INTEGER   default 0,
    "roundsToDrop"    INTEGER   default 0,
    "minimumRounds"   INTEGER   default 0,
    "groupPowers"     INTEGER   default 0,
    "unplayedScore"   INTEGER   default 0,
    "scalePercentage" DOUBLE    default 0,
    "assignPowers"    BOOLEAN,
    "scoreConflict"   INTEGER   default 0 not null,
    "groupId"         INTEGER   default NULL,
    "date"            TIMESTAMP default NULL
);

create index TOURNAMENT_ID
    on "Tournament" ("id");

create index TOURNAMENT_NUMHEATS
    on "Tournament" ("totalRounds");

create index TOURNAMENT_SYSTEMID
    on "Tournament" ("scoringSystemId");

create table "TournamentPlayer"
(
    "tournamentID" INTEGER default 0 not null,
    "playerID"     INTEGER default 0 not null,
    "roundNumbers" INTEGER default 0,
    primary key ("tournamentID", "playerID")
);

create index TOURNAMENTPLAYER_PLAYERID
    on "TournamentPlayer" ("playerID");

create index TOURNAMENTPLAYER_TOURNAMENTID
    on "TournamentPlayer" ("tournamentID");
