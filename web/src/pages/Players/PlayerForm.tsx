import React, { useState, useEffect } from 'react';
import {
  TextField,
  Grid,
  Chip,
  InputAdornment,
  IconButton,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import FormDialog from '../../components/Form/FormDialog';
import { Player } from '../../models/Player';

interface PlayerFormProps {
  open: boolean;
  onClose: () => void;
  onSubmit: (player: Player) => void;
  player: Player | null;
}

const PlayerForm: React.FC<PlayerFormProps> = ({
  open,
  onClose,
  onSubmit,
  player,
}) => {
  const [firstName, setFirstName] = useState('');
  const [lastName, setLastName] = useState('');
  const [email, setEmail] = useState('');
  const [emails, setEmails] = useState<string[]>([]);
  const [firstNameError, setFirstNameError] = useState('');
  const [lastNameError, setLastNameError] = useState('');

  useEffect(() => {
    if (player) {
      // If we have firstName and lastName, use them
      if (player.firstName && player.lastName) {
        setFirstName(player.firstName);
        setLastName(player.lastName);
      } else if (player.name) {
        // Otherwise, try to split the name
        const nameParts = player.name.split(' ');
        if (nameParts.length > 1) {
          setFirstName(nameParts[0]);
          setLastName(nameParts.slice(1).join(' '));
        } else {
          setFirstName(player.name);
          setLastName('');
        }
      }
      setEmails(player.emailAddresses || []);
    } else {
      setFirstName('');
      setLastName('');
      setEmails([]);
    }
    setEmail('');
    setFirstNameError('');
    setLastNameError('');
  }, [player, open]);

  const handleFirstNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFirstName(e.target.value);
    if (!e.target.value) {
      setFirstNameError('First name is required');
    } else {
      setFirstNameError('');
    }
  };

  const handleLastNameChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setLastName(e.target.value);
    if (!e.target.value) {
      setLastNameError('Last name is required');
    } else {
      setLastNameError('');
    }
  };

  const handleEmailChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
  };

  const handleAddEmail = () => {
    if (email && isValidEmail(email) && !emails.includes(email)) {
      setEmails([...emails, email]);
      setEmail('');
    }
  };

  const handleDeleteEmail = (emailToDelete: string) => {
    setEmails(emails.filter((e) => e !== emailToDelete));
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      e.preventDefault();
      handleAddEmail();
    }
  };

  const isValidEmail = (email: string) => {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
  };

  const handleSubmit = () => {
    if (!firstName) {
      setFirstNameError('First name is required');
      return;
    }
    if (!lastName) {
      setLastNameError('Last name is required');
      return;
    }

    const updatedPlayer: Player = {
      id: player?.id || 0,
      name: `${firstName} ${lastName}`, // Keep name for backward compatibility
      firstName,
      lastName,
      emailAddresses: emails.length > 0 ? emails : undefined,
    };

    onSubmit(updatedPlayer);
  };

  return (
    <FormDialog
      open={open}
      onClose={onClose}
      onSubmit={handleSubmit}
      title={player ? 'Edit Player' : 'Add Player'}
      disableSubmit={!firstName || !lastName}
    >
      <Grid container spacing={3}>
        <Grid
          size={{
            xs: 12,
            sm: 6
          }}>
          <TextField
            required
            fullWidth
            label="First Name"
            value={firstName}
            onChange={handleFirstNameChange}
            error={!!firstNameError}
            helperText={firstNameError}
          />
        </Grid>
        <Grid
          size={{
            xs: 12,
            sm: 6
          }}>
          <TextField
            required
            fullWidth
            label="Last Name"
            value={lastName}
            onChange={handleLastNameChange}
            error={!!lastNameError}
            helperText={lastNameError}
          />
        </Grid>
        <Grid size={12}>
          <TextField
            fullWidth
            label="Email"
            value={email}
            onChange={handleEmailChange}
            onKeyPress={handleKeyPress}
            error={email !== '' && !isValidEmail(email)}
            helperText={
              email !== '' && !isValidEmail(email) ? 'Invalid email format' : ''
            }
            InputProps={{
              endAdornment: (
                <InputAdornment position="end">
                  <IconButton
                    onClick={handleAddEmail}
                    disabled={
                      !email || !isValidEmail(email) || emails.includes(email)
                    }
                  >
                    <AddIcon />
                  </IconButton>
                </InputAdornment>
              ),
            }}
          />
        </Grid>
        <Grid size={12}>
          <div style={{ display: 'flex', flexWrap: 'wrap', gap: 8 }}>
            {emails.map((email, index) => (
              <Chip
                key={index}
                label={email}
                onDelete={() => handleDeleteEmail(email)}
              />
            ))}
          </div>
        </Grid>
      </Grid>
    </FormDialog>
  );
};

export default PlayerForm;
