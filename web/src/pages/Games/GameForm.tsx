import React, { useState, useEffect } from 'react';
import {
  TextField,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
  Button,
  FormControl,
  Grid,
  InputLabel,
  Select,
  TableContainer,
  IconButton,
  Autocomplete,
  Divider,
  MenuItem,
  FormHelperText,
  Typography,
  Paper,
  Tooltip,
  Alert,
  Table,
  TableHead,
  TableRow,
  TableCell,
  TableBody,
} from '@mui/material';
import { SelectChangeEvent } from '@mui/material/Select';
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import FormDialog from '../../components/Form/FormDialog';
import {
  Game,
  GameStatus,
  GamePlayer,
  GamePlayers,
  GameResult,
  Powers,
} from '../../models/Game';
import { ScoringSystem } from '../../models/ScoringSystem';
import { Player } from '../../models/Player';
import { playerService } from '../../services';

interface GameFormProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (game: Game) => void;
  game: Game | null;
}

const GameForm: React.FC<GameFormProps> = ({
  open,
  onClose,
  onSubmit,
  game,
}) => {
  const [name, setName] = useState('');
  const [status, setStatus] = useState<GameStatus>(GameStatus.Scheduled);
  const [tournamentId, setTournamentId] = useState<number | undefined>(
    undefined
  );
  const [tournamentName, setTournamentName] = useState<string | undefined>(
    undefined
  );
  const [round, setRound] = useState<number | undefined>(undefined);
  const [board, setBoard] = useState<number | undefined>(undefined);
  const [players, setPlayers] = useState<GamePlayers>([]);
  const [availablePlayers, setAvailablePlayers] = useState<Player[]>([]);
  const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);
  const [nameError, setNameError] = useState('');
  const [scoringSystems, setScoringSystems] = useState<ScoringSystem[]>([]);
  const [selectedScoringSystem, setSelectedScoringSystem] =
    useState<ScoringSystem | null>(null);
  const [gameInError, setGameInError] = useState(false);
  const [confirmationDialog, setConfirmationDialog] = useState<{
    open: boolean;
    title: string;
    message: string;
    onConfirm: () => void;
  }>({ open: false, title: '', message: '', onConfirm: () => {} });
  const [totalConflicts, setTotalConflicts] = useState(0);
  const [totalScore, setTotalScore] = useState(0);
  const [activeGameControl, setActiveGameControl] = useState(false);

  useEffect(() => {
    // Fetch players when the form opens
    if (open) {
      fetchPlayers();
      fetchScoringSystems();
    }
  }, [open]);

  useEffect(() => {
    // Reset form when game changes
    if (game) {
      setName(game.name);
      setStatus(game.status || GameStatus.Scheduled);
      setTournamentId(game.tournamentId);
      setTournamentName(game.tournamentName);
      setRound(game.round);
      setBoard(game.board);

      // Ensure players have complete properties
      const completePlayers = (game.players || []).map((player) => ({
        ...player,
        playComplete: isPlayerComplete(player),
        centers: player.centers || 0,
        years: player.years || 0,
        conflict: player.conflict || 0,
        conflictDetails: player.conflictDetails || [],
        score: player.score || 0,
      }));
      setPlayers(completePlayers);

      // Set scoring system
      setSelectedScoringSystem(game.scoringSystem || null);

      // Calculate game control active state
      updateGameControlState({
        ...game,
        players: completePlayers,
        scoringSystem: game.scoringSystem,
      });
    } else {
      setName('');
      setStatus(GameStatus.Scheduled);
      setTournamentId(undefined);
      setTournamentName(undefined);
      setRound(undefined);
      setBoard(undefined);
      setPlayers([]);
      setSelectedScoringSystem(null);
      setActiveGameControl(false);
    }
    setNameError('');
    setSelectedPlayer(null);
    setGameInError(false);
  }, [game, open]);

  // Effect to update conflicts when players change
  useEffect(() => {
    if (players.length > 0) {
      calculateConflicts();
    }
  }, [players]);

  // Effect to update total score when player scores change
  useEffect(() => {
    const total = players.reduce((sum, player) => sum + (player.score || 0), 0);
    setTotalScore(Math.round(total * 100) / 100);
  }, [players]);

  const fetchPlayers = async () => {
    try {
      const data = await playerService.getAll();
      setAvailablePlayers(data);
    } catch (err) {
      console.error('Failed to fetch players:', err);
    }
  };

  const fetchScoringSystems = async () => {
    try {
      const { scoringSystemService } = await import('../../services');
      const systems = await scoringSystemService.getAll();
      setScoringSystems(systems);

      // If we don't have a selected scoring system, use the default
      if (!selectedScoringSystem) {
        const defaultSystem = await scoringSystemService.getDefault();
        setSelectedScoringSystem(defaultSystem);
      }
    } catch (err) {
      console.error('Failed to fetch scoring systems:', err);
    }
  };

  // Check if a player has complete data
  const isPlayerComplete = (player: GamePlayer): boolean => {
    if (player.power === Powers.None) return false;
    if (player.result === GameResult.Unknown) return false;

    // For finished games, centers and years must be provided
    if (status === GameStatus.Finished) {
      if (player.centers === undefined || player.years === undefined) {
        return false;
      }
    }

    return true;
  };

  // Check if all powers are assigned in a game
  const allPowersAssigned = (game: Game): boolean => {
    return game.players?.every((p) => p.power !== Powers.None) ?? false;
  };

  // Check if the game data is complete
  const isGameDataComplete = (game: Game): boolean => {
    if (!game.players?.length) return false;

    return game.players.every((player) => {
      if (player.power === Powers.None) return false;
      if (player.result === GameResult.Unknown) return false;
      if (game.status === GameStatus.Finished) {
        if (player.centers === undefined || player.years === undefined) {
          return false;
        }
      }
      return true;
    });
  };

  // Calculate conflicts for all players in the game
  const calculateConflicts = () => {
    const gameService = require('../../services/gameService').gameService;

    let totalConflictScore = 0;
    const updatedPlayers = [...players];

    // Create a game object for conflict calculation
    const tempGame: Game = {
      id: game?.id || 0,
      name,
      status,
      tournamentId,
      tournamentName,
      round,
      board,
      players: updatedPlayers,
      scoringSystemId: selectedScoringSystem?.id,
      scoringSystem: selectedScoringSystem || undefined,
    };

    // Calculate conflicts for each player
    updatedPlayers.forEach((player) => {
      const conflict = gameService.calculateConflicts(player, tempGame);
      totalConflictScore += conflict;
    });

    setPlayers(updatedPlayers);
    setTotalConflicts(totalConflictScore);
  };

  // Update game control active state based on game state
  const updateGameControlState = (gameToCheck: Game) => {
    // Game control is active when:
    // 1. Game status is Underway
    // 2. All powers are assigned (no TBD)
    const powersAssigned = allPowersAssigned(gameToCheck);
    const active = gameToCheck.status === GameStatus.Underway && powersAssigned;
    setActiveGameControl(active);

    // Calculate scores if active and we have a scoring system
    if (active && gameToCheck.scoringSystem) {
      calculateScores(gameToCheck);
    }
  };

  // Calculate scores for the game
  const calculateScores = (gameToCheck: Game) => {
    const gameService = require('../../services/gameService').gameService;

    // Update player complete status
    const updatedPlayers =
      gameToCheck.players?.map((player) => ({
        ...player,
        playComplete: isPlayerComplete(player),
      })) || [];

    // Only calculate if we have a scoring system
    if (gameToCheck.scoringSystem) {
      const updated = {
        ...gameToCheck,
        players: updatedPlayers,
      };

      // Calculate scores
      const success = gameService.calculateScores(updated);

      // If calculation succeeded, update player scores
      if (success) {
        setPlayers(updated.players || []);
      } else {
        setGameInError(true);
      }
    }
  };

  const handleNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setName(e.target.value);
    if (!e.target.value) {
      setNameError('Name is required');
    } else {
      setNameError('');
    }
  };

  const handleStatusChange = (e: React.ChangeEvent<{ value: unknown }>) => {
    const newStatus = e.target.value as GameStatus;
    const currentStatus = status;

    // Validate status changes
    if (newStatus === GameStatus.Finished) {
      // Check if all required data is present for a finished game
      const gameToCheck: Game = {
        id: game?.id || 0,
        name,
        status: newStatus,
        players,
        scoringSystem: selectedScoringSystem || undefined,
      };

      if (!isGameDataComplete(gameToCheck)) {
        // Show error dialog
        setConfirmationDialog({
          open: true,
          title: 'Cannot Finish Game',
          message:
            'Game details are not complete. Please ensure all players have powers, results, centers, and years assigned.',
          onConfirm: () => {
            setConfirmationDialog((prev) => ({ ...prev, open: false }));
          },
        });
        return;
      }
    }
    // Check if changing from Underway to Seeded (unstarting the game)
    else if (
      currentStatus === GameStatus.Underway &&
      newStatus === GameStatus.Seeded
    ) {
      // If any game data exists, confirm before clearing
      const hasGameData = players.some(
        (p) =>
          p.centers !== undefined ||
          p.years !== undefined ||
          p.result !== GameResult.Unknown
      );

      if (hasGameData) {
        setConfirmationDialog({
          open: true,
          title: 'Confirm Erasure of Game-Player Details',
          message:
            'Are you sure you wish to unstart this game? The game details recorded for players will be erased.',
          onConfirm: () => {
            // Reset player data
            const resetPlayers = players.map((p) => ({
              ...p,
              centers: undefined,
              years: undefined,
              result: GameResult.Unknown,
              score: undefined,
              playComplete: false,
            }));

            setPlayers(resetPlayers);
            setStatus(newStatus);
            setConfirmationDialog((prev) => ({ ...prev, open: false }));
          },
        });
        return;
      }
    }

    // If we passed validation or no validation was needed
    setStatus(newStatus);

    // Update game control active state
    const updatedGame: Game = {
      id: game?.id || 0,
      name,
      status: newStatus,
      players,
      scoringSystem: selectedScoringSystem || undefined,
    };
    updateGameControlState(updatedGame);
  };

  const handleRoundChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = parseInt(e.target.value);
    setRound(isNaN(value) ? undefined : value);
  };

  const handleBoardChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = parseInt(e.target.value);
    setBoard(isNaN(value) ? undefined : value);
  };

  const handleAddPlayer = () => {
    if (
      selectedPlayer &&
      !players.some((p) => p.playerId === selectedPlayer.id)
    ) {
      const newPlayer: GamePlayer = {
        playerId: selectedPlayer.id,
        playerName: selectedPlayer.name,
        power: Powers.None,
        result: GameResult.Unknown,
        centers: 0,
        years: 0,
        playComplete: false,
        conflict: 0,
        conflictDetails: [],
      };

      const updatedPlayers = [...players, newPlayer];
      setPlayers(updatedPlayers);
      setSelectedPlayer(null);

      // Update game control state
      const updatedGame: Game = {
        id: game?.id || 0,
        name,
        status,
        players: updatedPlayers,
        scoringSystem: selectedScoringSystem || undefined,
      };
      updateGameControlState(updatedGame);
    }
  };

  const handleRemovePlayer = (playerId: number) => {
    setPlayers(players.filter((p) => p.playerId !== playerId));
  };

  const handlePowerChange = (playerId: number, power: Powers) => {
    // If this power is already assigned to another player, swap powers
    const otherPlayer = players.find(
      (p) => p.power === power && p.playerId !== playerId
    );

    let updatedPlayers = [...players];

    if (otherPlayer && power !== Powers.None) {
      // Swap powers with the other player
      updatedPlayers = players.map((p) => {
        if (p.playerId === playerId) {
          return { ...p, power };
        } else if (p.playerId === otherPlayer.playerId) {
          return { ...p, power: Powers.None };
        }
        return p;
      });
    }
    return updatedPlayers;
  };

  const handleResultChange = (playerId: number, result: GameResult) => {
    setPlayers(
      players.map((p) =>
        p.playerId === playerId
          ? {
              ...p,
              result,
              playComplete: isPlayerComplete({
                ...p,
                result,
              }),
            }
          : p
      )
    );

    // If game is active, recalculate scores when result changes
    if (activeGameControl) {
      const updatedGame = {
        id: game?.id || 0,
        name,
        status,
        players: players.map((p) =>
          p.playerId === playerId ? { ...p, result } : p
        ),
        scoringSystem: selectedScoringSystem || undefined,
      };
      calculateScores(updatedGame);
    }
  };

  const handleScoreChange = (playerId: number, scoreStr: string) => {
    const score = parseFloat(scoreStr);
    setPlayers(
      players.map((p) =>
        p.playerId === playerId
          ? { ...p, score: isNaN(score) ? undefined : score }
          : p
      )
    );
  };

  const handleCentersChange = (playerId: number, value: string) => {
    const centers = parseInt(value);
    setPlayers(
      players.map((p) => {
        if (p.playerId === playerId) {
          const updatedPlayer = {
            ...p,
            centers: isNaN(centers) ? undefined : centers,
            playComplete: isPlayerComplete({
              ...p,
              centers: isNaN(centers) ? undefined : centers,
            }),
          };
          return updatedPlayer;
        }
        return p;
      })
    );
  };

  const handleYearsChange = (playerId: number, value: string) => {
    const years = parseInt(value);
    setPlayers(
      players.map((p) => {
        if (p.playerId === playerId) {
          const updatedPlayer = {
            ...p,
            years: isNaN(years) ? undefined : years,
            playComplete: isPlayerComplete({
              ...p,
              years: isNaN(years) ? undefined : years,
            }),
          };
          return updatedPlayer;
        }
        return p;
      })
    );
  };

  const handleScoringSystemChange = (event: SelectChangeEvent<number>) => {
    const systemId = Number(event.target.value);
    const system = scoringSystems.find((s) => s.id === systemId);
    setSelectedScoringSystem(system || null);

    // Recalculate scores if game is active
    if (system && activeGameControl) {
      calculateScores({
        id: game?.id || 0,
        name,
        status,
        scoringSystem: system,
        players,
      });
    }
  };

  const handleSubmit = () => {
    if (!name) {
      setNameError('Name is required');
      return;
    }

    const updatedGame: Game = {
      id: game?.id || 0,
      name,
      status,
      tournamentId,
      tournamentName,
      round,
      board,
      players: players.length > 0 ? players : undefined,
      scoringSystemId: selectedScoringSystem?.id,
      scoringSystem: selectedScoringSystem || undefined,
    };

    onSubmit(updatedGame);
  };

  // Filter out players that are already in the game
  const filteredPlayers = availablePlayers.filter(
    (player) => !players.some((p) => p.playerId === player.id)
  );

  // Get power color based on the power
  const getPowerColor = (power: Powers) => {
    switch (power) {
      case Powers.Austria:
        return { color: 'white', backgroundColor: 'red' };
      case Powers.England:
        return { color: 'white', backgroundColor: 'royalblue' };
      case Powers.France:
        return { color: 'black', backgroundColor: 'skyblue' };
      case Powers.Germany:
        return { color: 'white', backgroundColor: 'black' };
      case Powers.Italy:
        return { color: 'black', backgroundColor: 'lime' };
      case Powers.Russia:
        return { color: 'black', backgroundColor: 'white' };
      case Powers.Turkey:
        return { color: 'black', backgroundColor: 'yellow' };
      default:
        return { color: 'inherit', backgroundColor: 'inherit' };
    }
  };

  return (
    <FormDialog
      open={open}
      onClose={onClose}
      onSubmit={handleSubmit}
      title={game ? 'Edit Game' : 'Add Game'}
      disableSubmit={!name}
      maxWidth="md"
    >
      <Grid container spacing={3}>
        <Grid
          size={{
            xs: 12,
            sm: 6,
          }}
        >
          <TextField
            required
            fullWidth
            label="Name"
            value={name}
            onChange={handleNameChange}
            error={!!nameError}
            helperText={nameError}
          />
        </Grid>
        <Grid
          size={{
            xs: 12,
            sm: 6,
          }}
        >
          <FormControl fullWidth>
            <InputLabel>Status</InputLabel>
            <Select value={status} onChange={handleStatusChange} label="Status">
              {Object.values(GameStatus).map((value) => (
                <MenuItem key={value} value={value}>
                  {value}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Grid>
        <Grid
          size={{
            xs: 12,
            sm: 6,
          }}
        >
          <TextField
            fullWidth
            label="Tournament"
            value={tournamentName || ''}
            disabled
            helperText="Tournament selection will be implemented later"
          />
        </Grid>
        <Grid
          size={{
            xs: 12,
            sm: 3,
          }}
        >
          <TextField
            fullWidth
            label="Round"
            type="number"
            value={round === undefined ? '' : round}
            onChange={handleRoundChange}
          />
        </Grid>
        <Grid
          size={{
            xs: 12,
            sm: 3,
          }}
        >
          <TextField
            fullWidth
            label="Board"
            type="number"
            value={board === undefined ? '' : board}
            onChange={handleBoardChange}
          />
        </Grid>

        <Grid
          size={{
            xs: 12,
            sm: 6,
          }}
        >
          <FormControl fullWidth>
            <InputLabel>Scoring System</InputLabel>
            <Select
              value={selectedScoringSystem?.id || ''}
              onChange={handleScoringSystemChange}
              label="Scoring System"
              disabled={status === GameStatus.Finished}
            >
              {scoringSystems.map((system) => (
                <MenuItem key={system.id} value={system.id}>
                  {system.name}
                </MenuItem>
              ))}
            </Select>
            {selectedScoringSystem?.isDefault && (
              <FormHelperText>Default Scoring System</FormHelperText>
            )}
          </FormControl>
        </Grid>

        <Grid size={12}>
          <Paper
            sx={{
              p: 2,
              backgroundColor: activeGameControl
                ? 'rgba(0, 255, 0, 0.1)'
                : 'inherit',
            }}
          >
            <Typography>
              Game Control: {activeGameControl ? 'Active' : 'Inactive'}
            </Typography>
            {gameInError && (
              <Tooltip title="Check that all required fields are filled and valid">
                <Alert severity="error" sx={{ mt: 1 }}>
                  Game data validation failed. Please check all player data.
                </Alert>
              </Tooltip>
            )}
            {!activeGameControl && status === GameStatus.Underway && (
              <Tooltip title="Assign powers to all players to activate game control">
                <Alert severity="warning" sx={{ mt: 1 }}>
                  Game control is inactive. All powers must be assigned.
                </Alert>
              </Tooltip>
            )}
          </Paper>
        </Grid>

        <Grid size={12}>
          <Divider sx={{ my: 2 }} />
          <Typography variant="subtitle1">Game Players</Typography>

          <Grid container spacing={2} sx={{ mb: 2 }}>
            <Grid size={9}>
              <Autocomplete
                options={filteredPlayers}
                getOptionLabel={(option) => option.name}
                value={selectedPlayer}
                onChange={(_, newValue) => setSelectedPlayer(newValue)}
                renderInput={(params) => (
                  <TextField {...params} label="Select Player" fullWidth />
                )}
              />
            </Grid>
            <Grid size={3}>
              <IconButton
                color="primary"
                onClick={handleAddPlayer}
                disabled={!selectedPlayer}
                sx={{ mt: 1 }}
              >
                <AddIcon />
              </IconButton>
            </Grid>
          </Grid>

          {players.length === 0 ? (
            <Typography variant="body2" color="textSecondary">
              No players added yet
            </Typography>
          ) : (
            <TableContainer component={Paper}>
              <Table size="small">
                <TableHead>
                  <TableRow>
                    <TableCell>Player</TableCell>
                    <TableCell>Power</TableCell>
                    <TableCell>Result</TableCell>
                    <TableCell>Centers</TableCell>
                    <TableCell>Years</TableCell>
                    <TableCell>Score</TableCell>
                    <TableCell>Actions</TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {players.map((player) => (
                    <TableRow key={player.playerId}>
                      <TableCell>{player.playerName}</TableCell>
                      <TableCell>
                        <FormControl fullWidth size="small">
                          <Select
                            value={player.power}
                            onChange={(e) =>
                              handlePowerChange(
                                player.playerId,
                                e.target.value as Powers
                              )
                            }
                            sx={getPowerColor(player.power)}
                          >
                            {Object.values(Powers).map((value) => (
                              <MenuItem
                                key={value}
                                value={value}
                                sx={getPowerColor(value)}
                              >
                                {value}
                              </MenuItem>
                            ))}
                          </Select>
                        </FormControl>
                      </TableCell>
                      <TableCell>
                        <FormControl fullWidth size="small">
                          <Select
                            value={player.result}
                            onChange={(e) =>
                              handleResultChange(
                                player.playerId,
                                e.target.value as GameResult
                              )
                            }
                          >
                            {Object.values(GameResult).map((value) => (
                              <MenuItem key={value} value={value}>
                                {value}
                              </MenuItem>
                            ))}
                          </Select>
                        </FormControl>
                      </TableCell>
                      <TableCell>
                        <TextField
                          size="small"
                          type="number"
                          label="Centers"
                          value={
                            player.centers === undefined ? '' : player.centers
                          }
                          onChange={(e) =>
                            handleCentersChange(player.playerId, e.target.value)
                          }
                          inputProps={{
                            min: 0,
                            max: 34,
                            step: 1,
                          }}
                          error={
                            player.centers !== undefined &&
                            (player.centers < 0 || player.centers > 34)
                          }
                          helperText={
                            player.centers !== undefined &&
                            (player.centers < 0 || player.centers > 34)
                              ? 'Centers must be between 0 and 34'
                              : ''
                          }
                          disabled={
                            status !== GameStatus.Underway &&
                            status !== GameStatus.Finished
                          }
                        />
                      </TableCell>
                      <TableCell>
                        <TextField
                          size="small"
                          type="number"
                          label="Years"
                          value={player.years === undefined ? '' : player.years}
                          onChange={(e) =>
                            handleYearsChange(player.playerId, e.target.value)
                          }
                          inputProps={{
                            min: 0,
                            step: 1,
                          }}
                          error={player.years !== undefined && player.years < 0}
                          helperText={
                            player.years !== undefined && player.years < 0
                              ? 'Years cannot be negative'
                              : ''
                          }
                          disabled={
                            status !== GameStatus.Underway &&
                            status !== GameStatus.Finished
                          }
                        />
                      </TableCell>
                      <TableCell>
                        <TextField
                          size="small"
                          type="number"
                          value={player.score === undefined ? '' : player.score}
                          onChange={(e) =>
                            handleScoreChange(player.playerId, e.target.value)
                          }
                          inputProps={{ step: 0.01 }}
                          disabled={!activeGameControl}
                        />
                      </TableCell>
                      <TableCell>
                        <IconButton
                          size="small"
                          onClick={() => handleRemovePlayer(player.playerId)}
                        >
                          <DeleteIcon />
                        </IconButton>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </TableContainer>
          )}

          {players.length > 0 &&
            !allPowersAssigned({ id: 0, name, status, players }) && (
              <Alert severity="info" sx={{ mt: 2 }}>
                Please assign powers to all players before starting the game.
              </Alert>
            )}
        </Grid>

        {players.length > 0 && (
          <Grid container spacing={2}>
            <Grid size={12}>
              <Paper sx={{ p: 2, mt: 2 }}>
                <Typography variant="h6">Conflicts</Typography>
                <Grid container spacing={2}>
                  {players.map((player) => (
                    <Grid
                      key={player.playerId}
                      size={{
                        xs: 12,
                        sm: 6,
                      }}
                    >
                      <Tooltip
                        title={
                          player.conflictDetails
                            ?.map((c) => `${c.reason} (${c.severity})`)
                            .join('\n') || ''
                        }
                      >
                        <Typography>
                          {player.playerName}: {player.conflict || 0} points
                        </Typography>
                      </Tooltip>
                    </Grid>
                  ))}
                  <Grid size={12}>
                    <Typography variant="subtitle1">
                      Total Conflicts: {totalConflicts} points
                    </Typography>
                  </Grid>
                </Grid>
              </Paper>
            </Grid>

            <Grid size={12}>
              <Paper sx={{ p: 2, mt: 2 }}>
                <Typography variant="h6">
                  Total Score: {totalScore.toFixed(2)}
                </Typography>
              </Paper>
            </Grid>
          </Grid>
        )}
      </Grid>
      <Dialog
        open={confirmationDialog.open}
        onClose={() =>
          setConfirmationDialog((prev) => ({ ...prev, open: false }))
        }
      >
        <DialogTitle>{confirmationDialog.title}</DialogTitle>
        <DialogContent>
          <DialogContentText>{confirmationDialog.message}</DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button
            onClick={() =>
              setConfirmationDialog((prev) => ({ ...prev, open: false }))
            }
          >
            Cancel
          </Button>
          <Button onClick={confirmationDialog.onConfirm} color="primary">
            Confirm
          </Button>
        </DialogActions>
      </Dialog>
    </FormDialog>
  );
};

export default GameForm;
