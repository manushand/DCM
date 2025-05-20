import React, { useState, useEffect } from 'react';
import {
  TextField,
  Grid,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Typography,
  Divider,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Button,
  IconButton,
  Box,
  Switch,
  FormControlLabel,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import AddIcon from '@mui/icons-material/Add';
import DeleteIcon from '@mui/icons-material/Delete';
import FormDialog from '../../components/Form/FormDialog';
import { Tournament, Round } from '../../models/Tournament';
import { Game } from '../../models/Game';
import { gameService } from '../../services';

interface TournamentFormProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (tournament: Tournament) => void;
  tournament: Tournament | null;
}

const TournamentForm: React.FC<TournamentFormProps> = ({
  open,
  onClose,
  onSubmit,
  tournament,
}) => {
  const [name, setName] = useState('');
  const [isEvent, setIsEvent] = useState(false);
  const [rounds, setRounds] = useState<Round[]>([]);
  const [games, setGames] = useState<Game[]>([]);
  const [nameError, setNameError] = useState('');

  useEffect(() => {
    // Fetch games when the form opens
    if (open) {
      fetchGames();
    }
  }, [open]);

  useEffect(() => {
    // Reset form when tournament changes
    if (tournament) {
      setName(tournament.name);
      setIsEvent(tournament.isEvent || false);
      setRounds(tournament.rounds || []);
    } else {
      setName('');
      setIsEvent(false);
      setRounds([]);
    }
    setNameError('');
  }, [tournament, open]);

  const fetchGames = async () => {
    try {
      const data = await gameService.getAll();
      setGames(data);
    } catch (err) {
      console.error('Failed to fetch games:', err);
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

  const handleIsEventChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setIsEvent(e.target.checked);
  };

  const handleAddRound = () => {
    const newRoundNumber =
      rounds.length > 0 ? Math.max(...rounds.map((r) => r.number)) + 1 : 1;

    const newRound: Round = {
      number: newRoundNumber,
      games: [],
    };

    setRounds([...rounds, newRound]);
  };

  const handleRemoveRound = (roundNumber: number) => {
    setRounds(rounds.filter((r) => r.number !== roundNumber));
  };

  const handleAddGameToRound = (roundNumber: number, game: Game) => {
    setRounds(
      rounds.map((round) => {
        if (round.number === roundNumber) {
          const gameExists = round.games?.some((g) => g.id === game.id);
          if (!gameExists) {
            return {
              ...round,
              games: [...(round.games || []), game],
            };
          }
        }
        return round;
      })
    );
  };

  const handleRemoveGameFromRound = (roundNumber: number, gameId: number) => {
    setRounds(
      rounds.map((round) => {
        if (round.number === roundNumber) {
          return {
            ...round,
            games: round.games?.filter((game) => game.id !== gameId) || [],
          };
        }
        return round;
      })
    );
  };

  const handleSubmit = () => {
    if (!name) {
      setNameError('Name is required');
      return;
    }

    const updatedTournament: Tournament = {
      id: tournament?.id || 0,
      name,
      isEvent,
      rounds: rounds.length > 0 ? rounds : undefined,
    };

    onSubmit(updatedTournament);
  };

  // Filter games that are not already in any round
  const getAvailableGames = (roundNumber: number) => {
    // Get all games that are already in rounds
    const gamesInOtherRounds = rounds
      .filter((r) => r.number !== roundNumber)
      .flatMap((r) => r.games || [])
      .map((g) => g.id);

    // Return games that are not in other rounds
    return games.filter((game) => !gamesInOtherRounds.includes(game.id));
  };

  return (
    <FormDialog
      open={open}
      onClose={onClose}
      onSubmit={handleSubmit}
      title={tournament ? 'Edit Tournament' : 'Add Tournament'}
      disableSubmit={!name}
      maxWidth="md"
    >
      <Grid container spacing={3}>
        <Grid item xs={12} sm={8}>
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
        <Grid item xs={12} sm={4}>
          <FormControlLabel
            control={
              <Switch
                checked={isEvent}
                onChange={handleIsEventChange}
                color="primary"
              />
            }
            label="Is Event"
          />
        </Grid>

        <Grid item xs={12}>
          <Divider sx={{ my: 2 }} />
          <Box
            display="flex"
            justifyContent="space-between"
            alignItems="center"
            mb={2}
          >
            <Typography variant="subtitle1">Rounds</Typography>
            <Button
              variant="outlined"
              startIcon={<AddIcon />}
              onClick={handleAddRound}
              size="small"
            >
              Add Round
            </Button>
          </Box>

          {rounds.length === 0 ? (
            <Typography variant="body2" color="textSecondary">
              No rounds added yet
            </Typography>
          ) : (
            rounds
              .sort((a, b) => a.number - b.number)
              .map((round) => (
                <Accordion key={round.number} sx={{ mb: 2 }}>
                  <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                    <Box
                      display="flex"
                      justifyContent="space-between"
                      width="100%"
                      alignItems="center"
                    >
                      <Typography>Round {round.number}</Typography>
                      <Box>
                        <Typography variant="body2" color="textSecondary">
                          {round.games?.length || 0} games
                        </Typography>
                        <IconButton
                          size="small"
                          onClick={(e) => {
                            e.stopPropagation();
                            handleRemoveRound(round.number);
                          }}
                        >
                          <DeleteIcon fontSize="small" />
                        </IconButton>
                      </Box>
                    </Box>
                  </AccordionSummary>
                  <AccordionDetails>
                    <Grid container spacing={2}>
                      <Grid item xs={12}>
                        <FormControl fullWidth size="small">
                          <InputLabel>Add Game</InputLabel>
                          <Select
                            value=""
                            label="Add Game"
                            onChange={(e) => {
                              const gameId = Number(e.target.value);
                              const game = games.find((g) => g.id === gameId);
                              if (game) {
                                handleAddGameToRound(round.number, game);
                              }
                            }}
                          >
                            <MenuItem value="">
                              <em>Select a game</em>
                            </MenuItem>
                            {getAvailableGames(round.number).map((game) => (
                              <MenuItem key={game.id} value={game.id}>
                                {game.name}
                              </MenuItem>
                            ))}
                          </Select>
                        </FormControl>
                      </Grid>
                      <Grid item xs={12}>
                        {round.games && round.games.length > 0 ? (
                          round.games.map((game) => (
                            <Box
                              key={game.id}
                              display="flex"
                              justifyContent="space-between"
                              alignItems="center"
                              p={1}
                              mb={1}
                              border="1px solid #eee"
                              borderRadius={1}
                            >
                              <Typography>{game.name}</Typography>
                              <IconButton
                                size="small"
                                onClick={() =>
                                  handleRemoveGameFromRound(
                                    round.number,
                                    game.id
                                  )
                                }
                              >
                                <DeleteIcon fontSize="small" />
                              </IconButton>
                            </Box>
                          ))
                        ) : (
                          <Typography variant="body2" color="textSecondary">
                            No games in this round
                          </Typography>
                        )}
                      </Grid>
                    </Grid>
                  </AccordionDetails>
                </Accordion>
              ))
          )}
        </Grid>
      </Grid>
    </FormDialog>
  );
};

export default TournamentForm;
