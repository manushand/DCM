import React from 'react';
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Navigate,
} from 'react-router-dom';
import { CssBaseline } from '@mui/material';

import Layout from './components/Layout/Layout';
import GamesPage from './pages/Games/GamesPage';
import GroupsPage from './pages/Groups/GroupsPage';
import PlayersPage from './pages/Players/PlayersPage';
import SettingsPage from './pages/Settings/SettingsPage';
import TournamentsPage from './pages/Tournaments/TournamentsPage';

function App() {
  return (
    <Router>
      <CssBaseline />
      <Layout>
        <Routes>
          <Route path="/" element={<Navigate to="/games" replace />} />
          <Route path="/games" element={<GamesPage />} />
          <Route path="/groups" element={<GroupsPage />} />
          <Route path="/players" element={<PlayersPage />} />
          <Route path="/settings" element={<SettingsPage />} />
          <Route path="/tournaments" element={<TournamentsPage />} />
        </Routes>
      </Layout>
    </Router>
  );
}

export default App;
