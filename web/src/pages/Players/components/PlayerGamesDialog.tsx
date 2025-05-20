import React, { useState, useEffect } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Typography,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Chip,
} from '@mui/material';
import { Player } from '../../../models/Player';
import { Game, GameStatus, Powers, GameResult } from '../../../models/Game';
import { playerService } from '../../../services';

interface PlayerGamesDialogProps {
  open: boolean;
  onClose: () => void;
  player: Player | null;
}

const PlayerGamesDialog: React.FC<PlayerGamesDialogProps> = ({
  open,
  onClose,
  player,
}) => {
  const [games, setGames] = useState<Game[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (open && player) {
      fetchGames();
    }
  }, [open, player]);

  const fetchGames = async () => {
    if (!player) return;

    try {
      setLoading(true);
      const data = await playerService.getPlayerGames(player.id);
      setGames(data);
      setError(null);
    } catch (err) {
      setError('Failed to fetch player games');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const getPlayerName = (p: Player) => {
    if (p.firstName && p.lastName) {
      return `${p.firstName} ${p.lastName}`;
    }
    return p.name;
  };

  // Function to get color for status chip
  const getStatusColor = (status: GameStatus) => {
    switch (status) {
      case GameStatus.Scheduled:
        return 'primary';
      case GameStatus.Underway:
        return 'warning';
      case GameStatus.Finished:
        return 'success';
      case GameStatus.Cancelled:
        return 'error';
      default:
        return 'default';
    }
  };

  // Function to get color for power
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

  // Function to get color for result chip
  const getResultColor = (result: GameResult) => {
    switch (result) {
      case GameResult.Win:
        return 'success';
      case GameResult.Draw:
        return 'info';
      case GameResult.Loss:
        return 'error';
      default:
        return 'default';
    }
  };

  // Find player's power and result in a game
  const getPlayerGameInfo = (game: Game) => {
    if (!player || !game.players)
      return { power: Powers.Unknown, result: GameResult.Unknown };

    const gamePlayer = game.players.find((p) => p.playerId === player.id);
    return {
      power: gamePlayer?.power || Powers.Unknown,
      result: gamePlayer?.result || GameResult.Unknown,
      score: gamePlayer?.score,
    };
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="lg" fullWidth>
      <DialogTitle>
        {player ? `Games for ${getPlayerName(player)}` : 'Player Games'}
      </DialogTitle>
      <DialogContent dividers>
        {error && (
          <Paper
            sx={{
              p: 2,
              mb: 2,
              bgcolor: 'error.light',
              color: 'error.contrastText',
            }}
          >
            <Typography>{error}</Typography>
          </Paper>
        )}

        {loading ? (
          <Typography>Loading games...</Typography>
        ) : games.length === 0 ? (
          <Typography color="textSecondary">
            No games found for this player
          </Typography>
        ) : (
          <TableContainer component={Paper}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Game</TableCell>
                  <TableCell>Tournament</TableCell>
                  <TableCell>Round</TableCell>
                  <TableCell>Board</TableCell>
                  <TableCell>Status</TableCell>
                  <TableCell>Power</TableCell>
                  <TableCell>Result</TableCell>
                  <TableCell>Score</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {games.map((game) => {
                  const { power, result, score } = getPlayerGameInfo(game);
                  return (
                    <TableRow key={game.id}>
                      <TableCell>{game.name}</TableCell>
                      <TableCell>{game.tournamentName || 'N/A'}</TableCell>
                      <TableCell align="center">
                        {game.round || 'N/A'}
                      </TableCell>
                      <TableCell align="center">
                        {game.board || 'N/A'}
                      </TableCell>
                      <TableCell>
                        <Chip
                          label={game.status}
                          color={getStatusColor(game.status)}
                          size="small"
                        />
                      </TableCell>
                      <TableCell>
                        <Chip
                          label={power}
                          size="small"
                          sx={getPowerColor(power)}
                        />
                      </TableCell>
                      <TableCell>
                        <Chip
                          label={result}
                          color={getResultColor(result)}
                          size="small"
                        />
                      </TableCell>
                      <TableCell>
                        {score !== undefined ? score.toFixed(2) : 'N/A'}
                      </TableCell>
                    </TableRow>
                  );
                })}
              </TableBody>
            </Table>
          </TableContainer>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="primary">
          Close
        </Button>
      </DialogActions>
    </Dialog>
  );
};

export default PlayerGamesDialog;
