import React, { useState, useEffect } from 'react';
import {
  Grid,
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  FormHelperText,
  CircularProgress,
  Autocomplete,
  Alert,
  Tooltip,
  Box,
} from '@mui/material';
import { SelectChangeEvent } from '@mui/material/Select';
import { Tournament } from '../../models/Tournament';
import { tournamentService } from '../../services';
import { Game } from '../../models/Game';

interface TournamentSelectorProps {
  game: Game | null;
  tournamentId: number | undefined;
  tournamentName: string | undefined;
  round: number | undefined;
  board: number | undefined;
  onTournamentChange: (
    tournamentId: number | undefined,
    tournamentName: string | undefined
  ) => void;
  onRoundChange: (round: number | undefined) => void;
  onBoardChange: (board: number | undefined) => void;
}

interface RoundInfo {
  number: number;
  name: string;
  boardCount: number;
  availableBoards: number[];
}

const TournamentSelector: React.FC<TournamentSelectorProps> = ({
  game,
  tournamentId,
  tournamentName,
  round,
  board,
  onTournamentChange,
  onRoundChange,
  onBoardChange,
}) => {
  const [tournaments, setTournaments] = useState<Tournament[]>([]);
  const [selectedTournament, setSelectedTournament] =
    useState<Tournament | null>(null);
  const [rounds, setRounds] = useState<RoundInfo[]>([]);
  const [availableBoards, setAvailableBoards] = useState<number[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [validationMessage, setValidationMessage] = useState<string | null>(
    null
  );
  const [validationSeverity, setValidationSeverity] = useState<
    'error' | 'warning' | 'info' | 'success'
  >('info');

  // Fetch active tournaments when component mounts
  useEffect(() => {
    fetchTournaments();
  }, []);

  // When tournament ID changes, fetch the tournament details
  useEffect(() => {
    if (tournamentId) {
      fetchTournamentDetails(tournamentId);
    }
  }, [tournamentId]);

  // When round changes, fetch available boards
  useEffect(() => {
    if (tournamentId && round !== undefined) {
      fetchAvailableBoards(tournamentId, round);
    }
  }, [tournamentId, round]);

  // When board changes, validate the board selection
  useEffect(() => {
    if (tournamentId && round !== undefined && board !== undefined) {
      validateBoardSelection(tournamentId, round, board);
    }
  }, [tournamentId, round, board]);

  const fetchTournaments = async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await tournamentService.getActiveTournaments();
      setTournaments(data);

      // If we have a tournamentId from props, select it
      if (tournamentId) {
        const tournament = data.find((t) => t.id === tournamentId);
        if (tournament) {
          setSelectedTournament(tournament);
        }
      }
    } catch (err) {
      console.error('Failed to fetch tournaments:', err);
      setError('Failed to load tournaments. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const fetchTournamentDetails = async (id: number) => {
    try {
      setLoading(true);
      setError(null);

      // Get tournament rounds
      const roundsInfo = await tournamentService.getRounds(id);
      setRounds(roundsInfo);

      // If we have a round from props, fetch available boards
      if (round !== undefined) {
        fetchAvailableBoards(id, round);
      }
    } catch (err) {
      console.error('Failed to fetch tournament details:', err);
      setError('Failed to load tournament details. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const fetchAvailableBoards = async (tournId: number, roundNum: number) => {
    try {
      setLoading(true);
      setError(null);
      const boards = await tournamentService.getAvailableBoards(
        tournId,
        roundNum
      );
      setAvailableBoards(boards);

      // If we have a board from props, validate it
      if (board !== undefined) {
        validateBoardSelection(tournId, roundNum, board);
      }
    } catch (err) {
      console.error('Failed to fetch available boards:', err);
      setError('Failed to load available boards. Please try again.');
    } finally {
      setLoading(false);
    }
  };

  const validateBoardSelection = async (
    tournId: number,
    roundNum: number,
    boardNum: number
  ) => {
    // If this is an existing game, the board is already assigned so consider it valid
    if (game?.id) {
      setValidationMessage(null);
      return;
    }

    // Check if the board is in the available boards list
    if (availableBoards.length > 0 && !availableBoards.includes(boardNum)) {
      setValidationMessage(
        `Board ${boardNum} is already in use or out of range for this round.`
      );
      setValidationSeverity('warning');
    } else {
      setValidationMessage(null);
    }
  };

  const handleTournamentChange = (tournament: Tournament | null) => {
    setSelectedTournament(tournament);

    // Reset round and board when tournament changes
    if (tournament) {
      onTournamentChange(tournament.id, tournament.name);
      setRounds([]);
      setAvailableBoards([]);
      onRoundChange(undefined);
      onBoardChange(undefined);

      // Fetch tournament details for the new selection
      fetchTournamentDetails(tournament.id);
    } else {
      onTournamentChange(undefined, undefined);
      setRounds([]);
      setAvailableBoards([]);
      onRoundChange(undefined);
      onBoardChange(undefined);
    }
  };

  const handleRoundChange = (event: SelectChangeEvent<number>) => {
    const roundNum = Number(event.target.value);
    onRoundChange(roundNum);

    // Reset board when round changes
    onBoardChange(undefined);

    // Fetch available boards for the new round
    if (tournamentId && roundNum !== undefined) {
      fetchAvailableBoards(tournamentId, roundNum);
    }
  };

  const handleBoardChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const boardNum = parseInt(event.target.value);
    onBoardChange(isNaN(boardNum) ? undefined : boardNum);

    // Validate the board selection
    if (tournamentId && round !== undefined && !isNaN(boardNum)) {
      validateBoardSelection(tournamentId, round, boardNum);
    }
  };

  const suggestNextAvailableBoard = async () => {
    if (tournamentId && round !== undefined) {
      try {
        setLoading(true);
        const nextBoard = await tournamentService.getNextAvailableBoard(
          tournamentId,
          round
        );
        if (nextBoard !== null) {
          onBoardChange(nextBoard);
        }
      } catch (err) {
        console.error('Failed to get next available board:', err);
      } finally {
        setLoading(false);
      }
    }
  };

  return (
    <Grid container spacing={3}>
      <Grid item xs={12} sm={6}>
        <Autocomplete
          options={tournaments}
          getOptionLabel={(option) => option.name}
          value={selectedTournament}
          onChange={(_, newValue) => handleTournamentChange(newValue)}
          renderInput={(params) => (
            <TextField
              {...params}
              label="Tournament"
              fullWidth
              error={!!error}
              helperText={error}
            />
          )}
          loading={loading}
          disabled={!!game?.id} // Disable changing tournament for existing games
        />
        {loading && (
          <Box display="flex" justifyContent="center" mt={1}>
            <CircularProgress size={24} />
          </Box>
        )}
      </Grid>

      <Grid item xs={12} sm={3}>
        <FormControl fullWidth>
          <InputLabel>Round</InputLabel>
          <Select
            value={round === undefined ? '' : round}
            onChange={handleRoundChange}
            label="Round"
            disabled={!tournamentId || loading || !!game?.id} // Disable for existing games
          >
            {rounds.map((r) => (
              <MenuItem key={r.number} value={r.number}>
                {r.name || `Round ${r.number}`}
              </MenuItem>
            ))}
          </Select>
          {rounds.length === 0 && tournamentId && (
            <FormHelperText>No rounds available</FormHelperText>
          )}
        </FormControl>
      </Grid>

      <Grid item xs={12} sm={3}>
        <TextField
          fullWidth
          label="Board"
          type="number"
          value={board === undefined ? '' : board}
          onChange={handleBoardChange}
          disabled={!tournamentId || round === undefined || loading}
          error={!!validationMessage}
          helperText={
            validationMessage ||
            (availableBoards.length > 0
              ? `Available boards: ${availableBoards.join(', ')}`
              : '')
          }
          InputProps={{
            endAdornment: (
              <Tooltip title="Suggest next available board">
                <span>
                  <CircularProgress
                    size={20}
                    style={{
                      cursor: 'pointer',
                      opacity: loading ? 1 : 0,
                      transition: 'opacity 0.3s',
                    }}
                    onClick={suggestNextAvailableBoard}
                  />
                </span>
              </Tooltip>
            ),
          }}
        />
      </Grid>

      {validationMessage && (
        <Grid item xs={12}>
          <Alert severity={validationSeverity}>{validationMessage}</Alert>
        </Grid>
      )}
    </Grid>
  );
};

export default TournamentSelector;
