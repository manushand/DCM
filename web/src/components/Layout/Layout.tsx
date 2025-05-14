import React, { ReactNode, useState } from 'react';
import { AppBar, Toolbar, Typography, Container, Box, IconButton } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';
import Navigation from '../Navigation/Navigation';

interface LayoutProps {
  children: ReactNode;
}

const Layout: React.FC<LayoutProps> = ({ children }) => {
  const [navigationOpen, setNavigationOpen] = useState(false);

  const handleNavigationToggle = () => {
    setNavigationOpen(!navigationOpen);
  };

  return (
    <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
      <AppBar position="static">
        <Toolbar>
          <IconButton
            size="large"
            edge="start"
            color="inherit"
            aria-label="menu"
            sx={{ mr: 2 }}
            onClick={handleNavigationToggle}
          >
            <MenuIcon />
          </IconButton>
          <Typography variant="h6" component="div" sx={{ flexGrow: 1 }}>
            DCM - Diplomacy Championship Manager
          </Typography>
        </Toolbar>
      </AppBar>
      <Navigation open={navigationOpen} onClose={() => setNavigationOpen(false)} />
      <Container component="main" sx={{ mt: 4, mb: 4, flexGrow: 1 }}>
        {children}
      </Container>
      <Box component="footer" sx={{ p: 2, mt: 'auto', backgroundColor: 'primary.main', color: 'white' }}>
        <Typography variant="body2" align="center">
          DCM © {new Date().getFullYear()} ARMADA
        </Typography>
      </Box>
    </Box>
  );
};

export default Layout;
