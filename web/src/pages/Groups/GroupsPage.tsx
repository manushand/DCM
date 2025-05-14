import React, { useState, useEffect } from 'react';
import { Button, Typography, Box, Paper, IconButton, Tooltip, Dialog, DialogTitle, DialogContent, DialogActions, DialogContentText } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import SportsEsportsIcon from '@mui/icons-material/SportsEsports';
import DataGrid from '../../components/DataGrid/DataGrid';
import { Group } from '../../models/Group';
import { groupService } from '../../services/groupService';
import GroupForm from './GroupForm';
import GroupGamesDialog from './components/GroupGamesDialog';

const GroupsPage: React.FC = () => {
  const [groups, setGroups] = useState<Group[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [openForm, setOpenForm] = useState(false);
  const [openDeleteDialog, setOpenDeleteDialog] = useState(false);
  const [openGamesDialog, setOpenGamesDialog] = useState(false);
  const [selectedGroup, setSelectedGroup] = useState<Group | null>(null);

  const fetchGroups = async () => {
    try {
      setLoading(true);
      const data = await groupService.getAll();
      setGroups(data);
      setError(null);
    } catch (err) {
      setError('Failed to fetch groups. Please try again later.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchGroups();
  }, []);

  const handleAddGroup = () => {
    setSelectedGroup(null);
    setOpenForm(true);
  };

  const handleEditGroup = (group: Group) => {
    setSelectedGroup(group);
    setOpenForm(true);
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setSelectedGroup(null);
  };

  const handleOpenDeleteDialog = (group: Group) => {
    setSelectedGroup(group);
    setOpenDeleteDialog(true);
  };

  const handleCloseDeleteDialog = () => {
    setOpenDeleteDialog(false);
  };

  const handleOpenGamesDialog = (group: Group) => {
    setSelectedGroup(group);
    setOpenGamesDialog(true);
  };

  const handleCloseGamesDialog = () => {
    setOpenGamesDialog(false);
  };

  const handleDissolveGroup = async () => {
    if (!selectedGroup) return;

    try {
      await groupService.delete(selectedGroup.id);
      fetchGroups();
      setOpenDeleteDialog(false);
      setSelectedGroup(null);
    } catch (err) {
      console.error('Failed to dissolve group:', err);
      // Handle error (could show a snackbar or other notification)
    }
  };

  const handleSaveGroup = async (group: Group) => {
    try {
      if (group.id) {
        await groupService.update(group.id, group);
      } else {
        await groupService.create(group);
      }
      fetchGroups();
      setOpenForm(false);
    } catch (err) {
      console.error('Failed to save group:', err);
      // Handle error (could show a snackbar or other notification)
    }
  };

  const columns = [
    { id: 'id', label: 'ID', minWidth: 50 },
    { id: 'name', label: 'Name', minWidth: 170 },
    {
      id: 'members',
      label: 'Members',
      minWidth: 170,
      format: (value: any[]) => value ? `${value.length} members` : '0 members'
    },
    {
      id: 'actions',
      label: 'Actions',
      minWidth: 150,
      align: 'center' as const,
      format: (_: any, row: Group) => (
        <Box>
          <Tooltip title="Edit Group">
            <IconButton
              size="small"
              onClick={(e) => {
                e.stopPropagation();
                handleEditGroup(row);
              }}
            >
              <EditIcon />
            </IconButton>
          </Tooltip>
          <Tooltip title="View Games">
            <IconButton
              size="small"
              onClick={(e) => {
                e.stopPropagation();
                handleOpenGamesDialog(row);
              }}
            >
              <SportsEsportsIcon />
            </IconButton>
          </Tooltip>
          <Tooltip title="Dissolve Group">
            <IconButton
              size="small"
              onClick={(e) => {
                e.stopPropagation();
                handleOpenDeleteDialog(row);
              }}
            >
              <DeleteIcon />
            </IconButton>
          </Tooltip>
        </Box>
      )
    }
  ];

  return (
    <div>
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
        <Typography variant="h4">Groups</Typography>
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={handleAddGroup}
        >
          Add Group
        </Button>
      </Box>

      {error && (
        <Paper sx={{ p: 2, mb: 2, bgcolor: 'error.light', color: 'error.contrastText' }}>
          <Typography>{error}</Typography>
        </Paper>
      )}

      <DataGrid
        columns={columns}
        rows={groups}
        onRowClick={handleEditGroup}
      />

      <GroupForm
        open={openForm}
        onClose={handleCloseForm}
        onSubmit={handleSaveGroup}
        group={selectedGroup}
      />

      {/* Confirmation Dialog for Dissolving a Group */}
      <Dialog
        open={openDeleteDialog}
        onClose={handleCloseDeleteDialog}
        aria-labelledby="alert-dialog-title"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle id="alert-dialog-title">
          Confirm Group Dissolution
        </DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            Are you sure you want to dissolve the group "{selectedGroup?.name}"? This action cannot be undone.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDeleteDialog} color="primary">
            Cancel
          </Button>
          <Button onClick={handleDissolveGroup} color="error" autoFocus>
            Dissolve Group
          </Button>
        </DialogActions>
      </Dialog>

      {/* Group Games Dialog */}
      <GroupGamesDialog
        open={openGamesDialog}
        onClose={handleCloseGamesDialog}
        group={selectedGroup}
      />
    </div>
  );
};

export default GroupsPage;
