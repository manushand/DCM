import React, { useState, useEffect } from 'react';
import {
  TextField,
  Grid,
  List,
  ListItem,
  ListItemText,
  ListItemSecondaryAction,
  IconButton,
  Autocomplete,
  Divider,
  Typography,
} from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import FormDialog from '../../components/Form/FormDialog';
import { Group, GroupMember } from '../../models/Group';
import { Player } from '../../models/Player';
import { playerService } from '../../services';

interface GroupFormProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (group: Group) => void;
  group: Group | null;
}

const GroupForm: React.FC<GroupFormProps> = ({
  open,
  onClose,
  onSubmit,
  group,
}) => {
  const [name, setName] = useState('');
  const [nameError, setNameError] = useState('');
  const [members, setMembers] = useState<GroupMember[]>([]);
  const [players, setPlayers] = useState<Player[]>([]);
  const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);

  useEffect(() => {
    // Fetch players when the form opens
    if (open) {
      fetchPlayers();
    }
  }, [open]);

  useEffect(() => {
    // Reset form when group changes
    if (group) {
      setName(group.name);
      setMembers(group.members || []);
    } else {
      setName('');
      setMembers([]);
    }
    setNameError('');
    setSelectedPlayer(null);
  }, [group, open]);

  const fetchPlayers = async () => {
    try {
      const data = await playerService.getAll();
      setPlayers(data);
    } catch (err) {
      console.error('Failed to fetch players:', err);
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

  const getPlayerName = (player: Player) => {
    if (player.firstName && player.lastName) {
      return `${player.firstName} ${player.lastName}`;
    }
    return player.name;
  };

  const handleAddMember = () => {
    if (
      selectedPlayer &&
      !members.some((m) => m.playerId === selectedPlayer.id)
    ) {
      const newMember: GroupMember = {
        playerId: selectedPlayer.id,
        playerName: getPlayerName(selectedPlayer),
      };
      setMembers([...members, newMember]);
      setSelectedPlayer(null);
    }
  };

  const handleRemoveMember = (playerId: number) => {
    setMembers(members.filter((m) => m.playerId !== playerId));
  };

  const handleSubmit = () => {
    if (!name) {
      setNameError('Name is required');
      return;
    }

    const updatedGroup: Group = {
      id: group?.id || 0,
      name,
      members: members.length > 0 ? members : undefined,
    };

    onSubmit(updatedGroup);
  };

  // Filter out players that are already members
  const availablePlayers = players.filter(
    (player) => !members.some((member) => member.playerId === player.id)
  );

  return (
    <FormDialog
      open={open}
      onClose={onClose}
      onSubmit={handleSubmit}
      title={group ? 'Edit Group' : 'Add Group'}
      disableSubmit={!name}
    >
      <Grid container spacing={3}>
        <Grid size={12}>
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
        <Grid size={12}>
          <Typography variant="subtitle1">Add Members</Typography>
          <Grid container spacing={2}>
            <Grid size={9}>
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
            <Grid size={3}>
              <IconButton
                color="primary"
                onClick={handleAddMember}
                disabled={!selectedPlayer}
                sx={{ mt: 1 }}
              >
                <DeleteIcon sx={{ transform: 'rotate(45deg)' }} />
              </IconButton>
            </Grid>
          </Grid>
        </Grid>
        <Grid size={12}>
          <Divider sx={{ my: 2 }} />
          <Typography variant="subtitle1">Members</Typography>
          {members.length === 0 ? (
            <Typography variant="body2" color="textSecondary">
              No members added yet
            </Typography>
          ) : (
            <List>
              {members.map((member) => (
                <ListItem key={member.playerId}>
                  <ListItemText primary={member.playerName} />
                  <ListItemSecondaryAction>
                    <IconButton
                      edge="end"
                      aria-label="delete"
                      onClick={() => handleRemoveMember(member.playerId)}
                    >
                      <DeleteIcon />
                    </IconButton>
                  </ListItemSecondaryAction>
                </ListItem>
              ))}
            </List>
          )}
        </Grid>
      </Grid>
    </FormDialog>
  );
};

export default GroupForm;
