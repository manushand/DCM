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
  Chip
} from '@mui/material';
import { Group } from '../../../models/Group';
import { Game, GameStatus, Powers, GameResult } from '../../../models/Game';
import { groupService } from '../../../services/groupService';

interface GroupGamesDialogProps {
  open: boolean;
  onClose: () => void;
  group: Group | null;
}

const GroupGamesDialog: React.FC<GroupGamesDialogProps> = ({ open, onClose, group }) => {
  const [games, setGames] = useState<Game[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (open && group) {
      fetchGames();
    }
  }, [open, group]);

  const fetchGames = async () => {
    if (!group) return;

    try {
      setLoading(true);
      const data = await groupService.getGroupGames(group.id);
      setGames(data);
      setError(null);
    } catch (err) {
      setError('Failed to fetch group games');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  // Function to get color for status chip
  const getStatusColor = (status: GameStatus) => {
    switch (status) {
      case GameStatus.Scheduled:
        return 'primary';
      case GameStatus.InProgress:
        return 'warning';
      case GameStatus.Completed:
        return 'success';
      case GameStatus.Cancelled:
        return 'error';
      default:
        return 'default';
    }
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="lg" fullWidth>
      <DialogTitle>
        {group ? `Games for ${group.name}` : 'Group Games'}
      </DialogTitle>
      <DialogContent dividers>
        {error && (
          <Paper sx={{ p: 2, mb: 2, bgcolor: 'error.light', color: 'error.contrastText' }}>
            <Typography>{error}</Typography>
          </Paper>
        )}

        {loading ? (
          <Typography>Loading games...</Typography>
        ) : games.length === 0 ? (
          <Typography color="textSecondary">No games found for this group</Typography>
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
                  <TableCell>Players</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {games.map((game) => (
                  <TableRow key={game.id}>
                    <TableCell>{game.name}</TableCell>
                    <TableCell>{game.tournamentName || 'N/A'}</TableCell>
                    <TableCell align="center">{game.round || 'N/A'}</TableCell>
                    <TableCell align="center">{game.board || 'N/A'}</TableCell>
                    <TableCell>
                      <Chip
                        label={game.status}
                        color={getStatusColor(game.status)}
                        size="small"
                      />
                    </TableCell>
                    <TableCell>
                      {game.players ? `${game.players.length} players` : '0 players'}
                    </TableCell>
                  </TableRow>
                ))}
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

export default GroupGamesDialog;