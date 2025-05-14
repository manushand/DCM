import React, { useState, useEffect } from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Typography,
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
  IconButton,
  Divider,
  TextField,
  Box,
  Autocomplete,
  Slider,
  Grid,
  Paper
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import { Player } from '../../../models/Player';
import { playerService, PlayerConflict } from '../../../services/playerService';

interface PlayerConflictsDialogProps {
  open: boolean;
  onClose: () => void;
  player: Player | null;
}

const PlayerConflictsDialog: React.FC<PlayerConflictsDialogProps> = ({ open, onClose, player }) => {
  const [conflicts, setConflicts] = useState<PlayerConflict[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [availablePlayers, setAvailablePlayers] = useState<Player[]>([]);
  const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);
  const [conflictValue, setConflictValue] = useState<number>(0);

  useEffect(() => {
    if (open && player) {
      fetchConflicts();
      fetchPlayers();
    }
  }, [open, player]);

  const fetchConflicts = async () => {
    if (!player) return;

    try {
      setLoading(true);
      const data = await playerService.getPlayerConflicts(player.id);
      setConflicts(data);
      setError(null);
    } catch (err) {
      setError('Failed to fetch player conflicts');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const fetchPlayers = async () => {
    try {
      const data = await playerService.getAll();
      // Filter out the current player and players that already have conflicts
      const filteredPlayers = data.filter(p =>
        p.id !== player?.id &&
        !conflicts.some(c => c.player.id === p.id)
      );
      setAvailablePlayers(filteredPlayers);
    } catch (err) {
      console.error('Failed to fetch players:', err);
    }
  };

  const handleAddConflict = async () => {
    if (!player || !selectedPlayer) return;

    try {
      const newConflict = await playerService.updatePlayerConflict(
        player.id,
        selectedPlayer.id,
        conflictValue
      );
      setConflicts([...conflicts, newConflict]);
      setSelectedPlayer(null);
      setConflictValue(0);
      fetchPlayers(); // Refresh available players
    } catch (err) {
      console.error('Failed to add conflict:', err);
    }
  };

  const handleUpdateConflict = async (conflictPlayer: Player, value: number) => {
    if (!player) return;

    try {
      await playerService.updatePlayerConflict(player.id, conflictPlayer.id, value);
      setConflicts(
        conflicts.map(c =>
          c.player.id === conflictPlayer.id
            ? { ...c, value }
            : c
        )
      );
    } catch (err) {
      console.error('Failed to update conflict:', err);
    }
  };

  const handleRemoveConflict = async (conflictPlayer: Player) => {
    if (!player) return;

    try {
      await playerService.updatePlayerConflict(player.id, conflictPlayer.id, 0);
      setConflicts(conflicts.filter(c => c.player.id !== conflictPlayer.id));
      fetchPlayers(); // Refresh available players
    } catch (err) {
      console.error('Failed to remove conflict:', err);
    }
  };

  const getPlayerName = (p: Player) => {
    if (p.firstName && p.lastName) {
      return `${p.firstName} ${p.lastName}`;
    }
    return p.name;
  };

  return (
    <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
      <DialogTitle>
        {player ? `Conflicts for ${getPlayerName(player)}` : 'Player Conflicts'}
      </DialogTitle>
      <DialogContent dividers>
        {error && (
          <Paper sx={{ p: 2, mb: 2, bgcolor: 'error.light', color: 'error.contrastText' }}>
            <Typography>{error}</Typography>
          </Paper>
        )}

        <Box mb={3}>
          <Typography variant="h6" gutterBottom>Add New Conflict</Typography>
          <Grid container spacing={2} alignItems="center">
            <Grid item xs={6}>
              <Autocomplete
                options={availablePlayers}
                getOptionLabel={getPlayerName}
                value={selectedPlayer}
                onChange={(_, newValue) => setSelectedPlayer(newValue)}
                renderInput={(params) => (
                  <TextField {...params} label="Select Player" fullWidth />
                )}
              />
            </Grid>
            <Grid item xs={4}>
              <Typography gutterBottom>Conflict Value: {conflictValue}</Typography>
              <Slider
                value={conflictValue}
                onChange={(_, newValue) => setConflictValue(newValue as number)}
                min={0}
                max={10}
                step={1}
                marks
                valueLabelDisplay="auto"
              />
            </Grid>
            <Grid item xs={2}>
              <Button
                variant="contained"
                color="primary"
                startIcon={<AddIcon />}
                onClick={handleAddConflict}
                disabled={!selectedPlayer}
                fullWidth
              >
                Add
              </Button>
            </Grid>
          </Grid>
        </Box>

        <Divider sx={{ my: 2 }} />

        <Typography variant="h6" gutterBottom>Current Conflicts</Typography>
        {conflicts.length === 0 ? (
          <Typography color="textSecondary">No conflicts found</Typography>
        ) : (
          <List>
            {conflicts.map((conflict) => (
              <ListItem key={conflict.player.id}>
                <ListItemText
                  primary={getPlayerName(conflict.player)}
                />
                <Box sx={{ width: 200, mx: 2 }}>
                  <Slider
                    value={conflict.value}
                    onChange={(_, newValue) =>
                      handleUpdateConflict(conflict.player, newValue as number)
                    }
                    min={0}
                    max={10}
                    step={1}
                    marks
                    valueLabelDisplay="auto"
                  />
                </Box>
                <ListItemSecondaryAction>
                  <IconButton
                    edge="end"
                    aria-label="delete"
                    onClick={() => handleRemoveConflict(conflict.player)}
                  >
                    <DeleteIcon />
                  </IconButton>
                </ListItemSecondaryAction>
              </ListItem>
            ))}
          </List>
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

export default PlayerConflictsDialog;