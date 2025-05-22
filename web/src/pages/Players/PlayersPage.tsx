import React, {useState, useEffect, JSX, useCallback} from 'react';
import {
  Button,
  Typography,
  Box,
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
import {normalizePlayerName} from "../../utils/playerUtils";
import Loading from "../../components/Loading/Loading";

const PlayersPage: React.FC = () => {
  const [players, setPlayers] = useState<Player[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [openForm, setOpenForm] = useState(false);
  const [openConflictsDialog, setOpenConflictsDialog] = useState(false);
  const [openGamesDialog, setOpenGamesDialog] = useState(false);
  const [openGroupsDialog, setOpenGroupsDialog] = useState(false);
  const [selectedPlayer, setSelectedPlayer] = useState<Player | null>(null);

  const fetchPlayers = useCallback(async () => {
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
  }, [setLoading, setPlayers, setError]);

  useEffect(() => {
    fetchPlayers().then();
  }, [fetchPlayers]);

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
      await fetchPlayers();
      setOpenForm(false);
    } catch (err) {
      console.error('Failed to save player:', err);
      // Handle error (could show a snackbar or other notification)
    }
  };

  const renderActions = (player: Player) => (
      <Box>
        <Tooltip title="Player Conflicts">
          <IconButton
            size="small"
            onClick={(e) => {
              e.stopPropagation();
              handleOpenConflicts(player);
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
              handleOpenGames(player);
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
              handleOpenGroups(player);
            }}
          >
            <GroupsIcon />
          </IconButton>
        </Tooltip>
      </Box>
  )

  const columns = [
    { id: 'id', label: 'ID', minWidth: 50 },
    { id: 'firstName', label: 'First Name', minWidth: 120 },
    { id: 'lastName', label: 'Last Name', minWidth: 120 },
    {
      id: 'name',
      label: 'Full Name',
      minWidth: 170,
      format: (value: string) => value,
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
      format: (value: JSX.Element) => value,
    },
  ];

  const processedPlayers = players.map((player) => ({
    ...player,
    name: normalizePlayerName(player),
    actions: renderActions(player),
  }));

  return (
    <div>
      {loading ? (
        <Loading text="Loading players..." error={error}/>
      ) : error ? (
        <Box
          display="flex"
          flexDirection="column"
          alignItems="center"
          justifyContent="center"
          height="100vh"
        >
          <Typography variant="h6" color="error" mb={2}>
            {error}
          </Typography>
          <Button variant="contained" color="primary" onClick={fetchPlayers}>
            Retry
          </Button>
        </Box>
      ) : (
        <>
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

          <DataGrid
            columns={columns}
            rows={processedPlayers}
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
        </>
        )}
    </div>
  );
};

export default PlayersPage;
