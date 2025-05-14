import React, { useState, useEffect } from 'react';
import { Button, Typography, Box, Paper, Chip } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import DataGrid from '../../components/DataGrid/DataGrid';
import { Tournament } from '../../models/Tournament';
import { tournamentService } from '../../services/tournamentService';
import TournamentForm from './TournamentForm';

const TournamentsPage: React.FC = () => {
  const [tournaments, setTournaments] = useState<Tournament[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [openForm, setOpenForm] = useState(false);
  const [selectedTournament, setSelectedTournament] = useState<Tournament | null>(null);

  const fetchTournaments = async () => {
    try {
      setLoading(true);
      const data = await tournamentService.getAll();
      setTournaments(data);
      setError(null);
    } catch (err) {
      setError('Failed to fetch tournaments. Please try again later.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTournaments();
  }, []);

  const handleAddTournament = () => {
    setSelectedTournament(null);
    setOpenForm(true);
  };

  const handleEditTournament = (tournament: Tournament) => {
    setSelectedTournament(tournament);
    setOpenForm(true);
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setSelectedTournament(null);
  };

  const handleSaveTournament = async (tournament: Tournament) => {
    try {
      if (tournament.id) {
        await tournamentService.update(tournament.id, tournament);
      } else {
        await tournamentService.create(tournament);
      }
      fetchTournaments();
      setOpenForm(false);
    } catch (err) {
      console.error('Failed to save tournament:', err);
      // Handle error (could show a snackbar or other notification)
    }
  };

  const columns = [
    { id: 'id', label: 'ID', minWidth: 50 },
    { id: 'name', label: 'Name', minWidth: 170 },
    {
      id: 'isEvent',
      label: 'Type',
      minWidth: 120,
      format: (value: boolean) => (
        <Chip
          label={value ? 'Event' : 'Tournament'}
          color={value ? 'primary' : 'default'}
          size="small"
        />
      )
    },
    {
      id: 'rounds',
      label: 'Rounds',
      minWidth: 100,
      format: (value: any[]) => value ? `${value.length} rounds` : '0 rounds'
    }
  ];

  return (
    <div>
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
        <Typography variant="h4">Tournaments</Typography>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={handleAddTournament}
        >
          Add Tournament
        </Button>
      </Box>

      {error && (
        <Paper sx={{ p: 2, mb: 2, bgcolor: 'error.light', color: 'error.contrastText' }}>
          <Typography>{error}</Typography>
        </Paper>
      )}

      <DataGrid
        columns={columns}
        rows={tournaments}
        onRowClick={handleEditTournament}
      />

      <TournamentForm
        open={openForm}
        onClose={handleCloseForm}
        onSubmit={handleSaveTournament}
        tournament={selectedTournament}
      />
    </div>
  );
};

export default TournamentsPage;