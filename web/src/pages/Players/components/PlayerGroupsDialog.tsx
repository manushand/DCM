import React, {useState, useEffect, useCallback} from 'react';
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Typography,
  Paper,
  List,
  ListItem,
  ListItemText,
  Divider,
  Accordion,
  AccordionSummary,
  AccordionDetails,
  Box,
  Chip,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import { Player } from '../../../models/Player';
import { Group } from '../../../models/Group';
import { playerService } from '../../../services';
import { getPlayerName } from '../../../utils';

interface PlayerGroupsDialogProps {
  open: boolean;
  onClose: () => void;
  player: Player | null;
}

const PlayerGroupsDialog: React.FC<PlayerGroupsDialogProps> = ({
  open,
  onClose,
  player,
}) => {
  const [groups, setGroups] = useState<Group[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchGroups = useCallback(async () => {
    if (!player) return;

    try {
      setLoading(true);
      const data = await playerService.getPlayerGroups(player.id);
      setGroups(data);
      setError(null);
    } catch (err) {
      setError('Failed to fetch player groups');
      console.error(err);
    } finally {
      setLoading(false);
    }
  }, [setLoading, player, setGroups, setError]);

  useEffect(() => {
    if (open && player) {
      fetchGroups();
    }
  }, [open, player, fetchGroups]);

  return (
    <Dialog open={open} onClose={onClose} maxWidth="md" fullWidth>
      <DialogTitle>
        {player ? `Groups for ${getPlayerName(player)}` : 'Player Groups'}
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
          <Typography>Loading groups...</Typography>
        ) : groups.length === 0 ? (
          <Typography color="textSecondary">
            No groups found for this player
          </Typography>
        ) : (
          <div>
            <Typography variant="h6" gutterBottom>
              {player?.firstName} {player?.lastName} is a member of{' '}
              {groups.length} group{groups.length !== 1 ? 's' : ''}
            </Typography>

            {groups.map((group) => (
              <Accordion key={group.id} sx={{ mb: 2 }}>
                <AccordionSummary expandIcon={<ExpandMoreIcon />}>
                  <Typography variant="subtitle1">{group.name}</Typography>
                </AccordionSummary>
                <AccordionDetails>
                  <Typography variant="subtitle2" gutterBottom>
                    Members:
                  </Typography>
                  {group.members && group.members.length > 0 ? (
                    <List>
                      {group.members.map((member) => (
                        <React.Fragment key={member.playerId}>
                          <ListItem>
                            <ListItemText
                              primary={member.playerName}
                              secondary={
                                member.rating !== undefined
                                  ? `Rating: ${member.rating}`
                                  : 'No rating'
                              }
                            />
                            {member.playerId === player?.id && (
                              <Box ml={2}>
                                <Chip
                                  label="You"
                                  color="primary"
                                  size="small"
                                />
                              </Box>
                            )}
                          </ListItem>
                          <Divider component="li" />
                        </React.Fragment>
                      ))}
                    </List>
                  ) : (
                    <Typography color="textSecondary">
                      No members in this group
                    </Typography>
                  )}
                </AccordionDetails>
              </Accordion>
            ))}
          </div>
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

export default PlayerGroupsDialog;
