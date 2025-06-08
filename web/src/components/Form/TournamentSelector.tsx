import React, { useState, useEffect, useCallback } from 'react';
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
  Box,
} from '@mui/material';
import { Tournament } from '../../models/Tournament';
import { Game } from '../../models/Game';
import { tournamentService } from '../../services';

interface RoundInfo {
  number: number;
  name: string;
  boardCount: number;
  availableBoards: number[];
}

const useTournamentSelector = (
  game: Game | null,
  tournamentId: number | undefined,
  round: number | undefined,
  board: number | undefined
) => {
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

  const fetchTournaments = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);
      const data = await tournamentService.getActiveTournaments();
      setTournaments(data);

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
  }, [tournamentId]);

  const validateBoardSelection = useCallback(
    async (tournId: number, roundNum: number, boardNum: number) => {
      if (game?.id) {
        setValidationMessage(null);
        return;
      }

      if (availableBoards.length > 0 && !availableBoards.includes(boardNum)) {
        setValidationMessage(
          `Board ${boardNum} is already in use or out of range for this round.`
        );
        setValidationSeverity('warning');
      } else {
        setValidationMessage(null);
      }
    },
    [game?.id, availableBoards]
  );

  const fetchAvailableBoards = useCallback(
    async (tournId: number, roundNum: number) => {
      try {
        setLoading(true);
        setError(null);
        const boards = await tournamentService.getAvailableBoards(
          tournId,
          roundNum
        );
        setAvailableBoards(boards);

        if (board !== undefined) {
          validateBoardSelection(tournId, roundNum, board);
        }
      } catch (err) {
        console.error('Failed to fetch available boards:', err);
        setError('Failed to load available boards. Please try again.');
      } finally {
        setLoading(false);
      }
    },
    [board, validateBoardSelection]
  );

  const fetchTournamentDetails = useCallback(
    async (id: number) => {
      try {
        setLoading(true);
        setError(null);

        const roundsInfo = await tournamentService.getRounds(id);
        setRounds(roundsInfo);

        if (round !== undefined) {
          fetchAvailableBoards(id, round);
        }
      } catch (err) {
        console.error('Failed to fetch tournament details:', err);
        setError('Failed to load tournament details. Please try again.');
      } finally {
        setLoading(false);
      }
    },
    [round, fetchAvailableBoards]
  );

  return {
    tournaments,
    selectedTournament,
    rounds,
    availableBoards,
    loading,
    error,
    validationMessage,
    validationSeverity,
    fetchTournaments,
    fetchTournamentDetails,
    fetchAvailableBoards,
    validateBoardSelection,
    setSelectedTournament,
  };
};

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
  const {
    tournaments,
    selectedTournament,
    rounds,
    availableBoards,
    loading,
    error,
    validationMessage,
    validationSeverity,
    fetchTournaments,
    fetchTournamentDetails,
    fetchAvailableBoards,
    validateBoardSelection,
    setSelectedTournament,
  } = useTournamentSelector(game, tournamentId, round, board);

  useEffect(() => {
    fetchTournaments().then();
  }, [fetchTournaments]);

  useEffect(() => {
    if (tournamentId) {
      fetchTournamentDetails(tournamentId).then();
    }
  }, [tournamentId, fetchTournamentDetails]);

  useEffect(() => {
    if (tournamentId && round !== undefined) {
      fetchAvailableBoards(tournamentId, round).then();
    }
  }, [tournamentId, round, fetchAvailableBoards]);

  useEffect(() => {
    if (tournamentId && round !== undefined && board !== undefined) {
      validateBoardSelection(tournamentId, round, board).then();
    }
  }, [tournamentId, round, board, validateBoardSelection]);

  const handleTournamentChange = (tournament: Tournament | null) => {
    setSelectedTournament(tournament);

    if (tournament) {
      onTournamentChange(tournament.id, tournament.name);
      onRoundChange(undefined);
      onBoardChange(undefined);
      fetchTournamentDetails(tournament.id).then();
    } else {
      onTournamentChange(undefined, undefined);
      onRoundChange(undefined);
      onBoardChange(undefined);
    }
  };

  return (
    <Grid container spacing={3}>
      <Grid
        size={{
          xs: 12,
          sm: 6,
        }}
      >
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
      <Grid
        size={{
          xs: 12,
          sm: 3,
        }}
      >
        <FormControl fullWidth>
          <InputLabel>Round</InputLabel>
          <Select
            value={round === undefined ? '' : round}
            onChange={(e) => onRoundChange(Number(e.target.value))}
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
          onChange={(e) => onBoardChange(Number(e.target.value))}
          disabled={!tournamentId || round === undefined || loading}
          error={!!validationMessage}
          helperText={
            validationMessage ||
            (availableBoards.length > 0
              ? `Available boards: ${availableBoards.join(', ')}`
              : '')
          }
        />
      </Grid>
      {validationMessage && (
        <Grid size={12}>
          <Alert severity={validationSeverity}>{validationMessage}</Alert>
        </Grid>
      )}
    </Grid>
  );
};

export default TournamentSelector;
