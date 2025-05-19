import React, { useState, useEffect } from 'react';
import {
  Typography,
  Box,
  Paper,
  Grid,
  TextField,
  Button,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Divider,
  Alert,
  Snackbar,
} from '@mui/material';
import SaveIcon from '@mui/icons-material/Save';

enum DatabaseType {
  None = 'None',
  Access = 'Access',
  SqlServer = 'SqlServer',
}

interface Settings {
  databaseType: DatabaseType;
  databaseFile?: string;
  databaseConnectionString?: string;
  smtpHost?: string;
  smtpPort?: number;
  smtpUsername?: string;
  smtpPassword?: string;
  smtpSsl?: boolean;
  fromEmailAddress?: string;
  fromEmailName?: string;
  testEmailAddress?: string;
  testEmailOnly?: boolean;
}

const SettingsPage: React.FC = () => {
  const [settings, setSettings] = useState<Settings>({
    databaseType: DatabaseType.None,
  });
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [snackbarMessage, setSnackbarMessage] = useState('');
  const [snackbarSeverity, setSnackbarSeverity] = useState<'success' | 'error'>(
    'success'
  );

  useEffect(() => {
    // Load settings from localStorage
    const savedSettings = localStorage.getItem('dcmSettings');
    if (savedSettings) {
      try {
        setSettings(JSON.parse(savedSettings));
      } catch (err) {
        console.error('Failed to parse saved settings:', err);
      }
    }
  }, []);

  const handleSettingChange = (key: keyof Settings, value: any) => {
    setSettings((prev) => ({
      ...prev,
      [key]: value,
    }));
  };

  const handleSaveSettings = () => {
    try {
      localStorage.setItem('dcmSettings', JSON.stringify(settings));
      showSnackbar('Settings saved successfully', 'success');
    } catch (err) {
      console.error('Failed to save settings:', err);
      showSnackbar('Failed to save settings', 'error');
    }
  };

  const showSnackbar = (message: string, severity: 'success' | 'error') => {
    setSnackbarMessage(message);
    setSnackbarSeverity(severity);
    setSnackbarOpen(true);
  };

  const handleCloseSnackbar = () => {
    setSnackbarOpen(false);
  };

  return (
    <div>
      <Box
        display="flex"
        justifyContent="space-between"
        alignItems="center"
        mb={2}
      >
        <Typography variant="h4">Settings</Typography>
        <Button
          variant="contained"
          color="primary"
          startIcon={<SaveIcon />}
          onClick={handleSaveSettings}
        >
          Save Settings
        </Button>
      </Box>

      <Paper sx={{ p: 3 }}>
        <Typography variant="h6" gutterBottom>
          Database Settings
        </Typography>
        <Grid container spacing={3}>
          <Grid item xs={12} sm={6}>
            <FormControl fullWidth>
              <InputLabel>Database Type</InputLabel>
              <Select
                value={settings.databaseType}
                label="Database Type"
                onChange={(e) =>
                  handleSettingChange('databaseType', e.target.value)
                }
              >
                {Object.values(DatabaseType).map((type) => (
                  <MenuItem key={type} value={type}>
                    {type}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Grid>

          {settings.databaseType === DatabaseType.Access && (
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Database File"
                value={settings.databaseFile || ''}
                onChange={(e) =>
                  handleSettingChange('databaseFile', e.target.value)
                }
                helperText="Full path to the Access database file"
              />
            </Grid>
          )}

          {settings.databaseType === DatabaseType.SqlServer && (
            <Grid item xs={12}>
              <TextField
                fullWidth
                label="Connection String"
                value={settings.databaseConnectionString || ''}
                onChange={(e) =>
                  handleSettingChange(
                    'databaseConnectionString',
                    e.target.value
                  )
                }
                helperText="SQL Server connection string"
              />
            </Grid>
          )}
        </Grid>

        <Divider sx={{ my: 3 }} />

        <Typography variant="h6" gutterBottom>
          Email Settings
        </Typography>
        <Grid container spacing={3}>
          <Grid item xs={12} sm={8}>
            <TextField
              fullWidth
              label="SMTP Host"
              value={settings.smtpHost || ''}
              onChange={(e) => handleSettingChange('smtpHost', e.target.value)}
            />
          </Grid>
          <Grid item xs={12} sm={4}>
            <TextField
              fullWidth
              label="SMTP Port"
              type="number"
              value={settings.smtpPort || ''}
              onChange={(e) =>
                handleSettingChange(
                  'smtpPort',
                  parseInt(e.target.value) || undefined
                )
              }
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label="SMTP Username"
              value={settings.smtpUsername || ''}
              onChange={(e) =>
                handleSettingChange('smtpUsername', e.target.value)
              }
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label="SMTP Password"
              type="password"
              value={settings.smtpPassword || ''}
              onChange={(e) =>
                handleSettingChange('smtpPassword', e.target.value)
              }
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label="From Email Address"
              value={settings.fromEmailAddress || ''}
              onChange={(e) =>
                handleSettingChange('fromEmailAddress', e.target.value)
              }
            />
          </Grid>
          <Grid item xs={12} sm={6}>
            <TextField
              fullWidth
              label="From Email Name"
              value={settings.fromEmailName || ''}
              onChange={(e) =>
                handleSettingChange('fromEmailName', e.target.value)
              }
            />
          </Grid>
          <Grid item xs={12}>
            <TextField
              fullWidth
              label="Test Email Address"
              value={settings.testEmailAddress || ''}
              onChange={(e) =>
                handleSettingChange('testEmailAddress', e.target.value)
              }
              helperText="Email address for testing"
            />
          </Grid>
        </Grid>
      </Paper>

      <Snackbar
        open={snackbarOpen}
        autoHideDuration={6000}
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
      >
        <Alert
          onClose={handleCloseSnackbar}
          severity={snackbarSeverity}
          sx={{ width: '100%' }}
        >
          {snackbarMessage}
        </Alert>
      </Snackbar>
    </div>
  );
};

export default SettingsPage;
