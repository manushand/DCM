import React, { useState, useEffect } from 'react';
import { Button, Typography, Box, Paper, Chip } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import DataGrid from '../../components/DataGrid/DataGrid';
import { Game, GameStatus, Powers } from '../../models/Game';
import { gameService } from '../../services/gameService';
import GameForm from './GameForm';

const GamesPage: React.FC = () => {
  const [games, setGames] = useState<Game[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [openForm, setOpenForm] = useState(false);
  const [selectedGame, setSelectedGame] = useState<Game | null>(null);

  const fetchGames = async () => {
    try {
      setLoading(true);
      const data = await gameService.getAll();
      setGames(data);
      setError(null);
    } catch (err) {
      setError('Failed to fetch games. Please try again later.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchGames();
  }, []);

  const handleAddGame = () => {
    setSelectedGame(null);
    setOpenForm(true);
  };

  const handleEditGame = (game: Game) => {
    setSelectedGame(game);
    setOpenForm(true);
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setSelectedGame(null);
  };

  const handleSaveGame = async (game: Game) => {
    try {
      if (game.id) {
        await gameService.update(game.id, game);
      } else {
        await gameService.create(game);
      }
      fetchGames();
      setOpenForm(false);
    } catch (err) {
      console.error('Failed to save game:', err);
      // Handle error (could show a snackbar or other notification)
    }
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

  const columns = [
    { id: 'id', label: 'ID', minWidth: 50 },
    { id: 'name', label: 'Name', minWidth: 170 },
    {
      id: 'status',
      label: 'Status',
      minWidth: 120,
      format: (value: GameStatus) => (
        <Chip label={value} color={getStatusColor(value)} size="small" />
      ),
    },
    { id: 'tournamentName', label: 'Tournament', minWidth: 150 },
    { id: 'round', label: 'Round', minWidth: 80, align: 'center' as const },
    { id: 'board', label: 'Board', minWidth: 80, align: 'center' as const },
    {
      id: 'players',
      label: 'Players',
      minWidth: 100,
      format: (value: any[]) =>
        value ? `${value.length} players` : '0 players',
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
        <Typography variant="h4">Games</Typography>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={handleAddGame}
        >
          Add Game
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

      <DataGrid columns={columns} rows={games} onRowClick={handleEditGame} />

      <GameForm
        open={openForm}
        onClose={handleCloseForm}
        onSubmit={handleSaveGame}
        game={selectedGame}
      />
    </div>
  );
};

export default GamesPage;
