import React, { useState, useEffect } from 'react';
import {
  Button,
  Typography,
  Box,
  Paper,
  IconButton,
  Tooltip,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import GroupsIcon from '@mui/icons-material/Groups';
import SportsEsportsIcon from '@mui/icons-material/SportsEsports';
import PeopleAltIcon from '@mui/icons-material/PeopleAlt';
import DataGrid from '../../components/DataGrid/DataGrid';
import { Player } from '../../models/Player';
import { playerService } from '../../services';
import PlayerForm from './PlayerForm';
import PlayerConflictsDialog from './components/PlayerConflictsDialog';
import PlayerGamesDialog from './components/PlayerGamesDialog';
import PlayerGroupsDialog from './components/PlayerGroupsDialog';

const PlayersPage: React.FC = () => {
  const [players, setPlayers] = useState<Player[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [openForm, setOpenForm] = useState(false);
  const [openConflictsDialog, setOpenConflictsDialog] = useState(false);
  const [openGamesDialog, setOpenGamesDialog] = useState(false);
  const [openGroupsDialog, setOpenGroupsDialog] = useState(false);
  const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);

  const fetchPlayers = async () => {
    try {
      setLoading(true);
      const data = await playerService.getAll();
      setPlayers(data);
      setError(null);
    } catch (err) {
      setError('Failed to fetch players. Please try again later.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPlayers();
  }, []);

  const handleAddPlayer = () => {
    setSelectedPlayer(null);
    setOpenForm(true);
  };

  const handleEditPlayer = (player: Player) => {
    setSelectedPlayer(player);
    setOpenForm(true);
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setSelectedPlayer(null);
  };

  const handleOpenConflicts = (player: Player) => {
    setSelectedPlayer(player);
    setOpenConflictsDialog(true);
  };

  const handleCloseConflicts = () => {
    setOpenConflictsDialog(false);
  };

  const handleOpenGames = (player: Player) => {
    setSelectedPlayer(player);
    setOpenGamesDialog(true);
  };

  const handleCloseGames = () => {
    setOpenGamesDialog(false);
  };

  const handleOpenGroups = (player: Player) => {
    setSelectedPlayer(player);
    setOpenGroupsDialog(true);
  };

  const handleCloseGroups = () => {
    setOpenGroupsDialog(false);
  };

  const handleSavePlayer = async (player: Player) => {
    try {
      if (player.id) {
        await playerService.update(player.id, player);
      } else {
        await playerService.create(player);
      }
      fetchPlayers();
      setOpenForm(false);
    } catch (err) {
      console.error('Failed to save player:', err);
      // Handle error (could show a snackbar or other notification)
    }
  };

  const columns = [
    { id: 'id', label: 'ID', minWidth: 50 },
    { id: 'firstName', label: 'First Name', minWidth: 120 },
    { id: 'lastName', label: 'Last Name', minWidth: 120 },
    {
      id: 'name',
      label: 'Full Name',
      minWidth: 170,
      format: (value: string, row: Player) => {
        // If firstName and lastName are available, use them
        if (row.firstName && row.lastName) {
          return `${row.firstName} ${row.lastName}`;
        }
        // Otherwise fall back to name
        return value;
      },
    },
    {
      id: 'emailAddresses',
      label: 'Email',
      minWidth: 170,
      format: (value: string[]) => (value ? value.join(', ') : ''),
    },
    {
      id: 'actions',
      label: 'Actions',
      minWidth: 150,
      align: 'center' as const,
      format: (_: any, row: Player) => (
        <Box>
          <Tooltip title="Player Conflicts">
            <IconButton
              size="small"
              onClick={(e) => {
                e.stopPropagation();
                handleOpenConflicts(row);
              }}
            >
              <PeopleAltIcon />
            </IconButton>
          </Tooltip>
          <Tooltip title="Player Games">
            <IconButton
              size="small"
              onClick={(e) => {
                e.stopPropagation();
                handleOpenGames(row);
              }}
            >
              <SportsEsportsIcon />
            </IconButton>
          </Tooltip>
          <Tooltip title="Player Groups">
            <IconButton
              size="small"
              onClick={(e) => {
                e.stopPropagation();
                handleOpenGroups(row);
              }}
            >
              <GroupsIcon />
            </IconButton>
          </Tooltip>
        </Box>
      ),
    },
  ];

  return (
    <div>
      <Box
        display="flex"
        justifyContent="space-between"
        alignItems="center"
        mb={2}
      >
        <Typography variant="h4">Players</Typography>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={handleAddPlayer}
        >
          Add Player
        </Button>
      </Box>

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

      <DataGrid
        columns={columns}
        rows={players}
        onRowClick={handleEditPlayer}
      />

      <PlayerForm
        open={openForm}
        onClose={handleCloseForm}
        onSubmit={handleSavePlayer}
        player={selectedPlayer}
      />

      <PlayerConflictsDialog
        open={openConflictsDialog}
        onClose={handleCloseConflicts}
        player={selectedPlayer}
      />

      <PlayerGamesDialog
        open={openGamesDialog}
        onClose={handleCloseGames}
        player={selectedPlayer}
      />

      <PlayerGroupsDialog
        open={openGroupsDialog}
        onClose={handleCloseGroups}
        player={selectedPlayer}
      />
    </div>
  );
};

export default PlayersPage;
