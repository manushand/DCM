# DCM Angular Site

This is the Angular web application that mimics the functionality of the MainForm found in the WinForms project (PC folder). It provides a web-based interface for the Diplomacy Competition Manager.

## Features

- **Main Form**: Mimics the WinForms MainForm with menu structure
- **Player Management**: Full CRUD operations for players
- **Player Conflicts**: Manage conflicts between players
- **Stubbed Functionality**: All non-player menu items are stubbed with placeholder alerts

## Prerequisites

- Node.js (v18 or later)
- Angular CLI (v19 or later)
- The API project must be running on localhost:5104

## Setup and Running

1. Navigate to the dcm-site directory:
   ```bash
   cd site/dcm-site
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   ng serve
   ```

4. Open your browser and navigate to `http://localhost:4200`

## API Integration

The application is configured to connect to the REST API running at `http://localhost:5104` through a proxy configuration to avoid CORS issues. Make sure the API project is running before using the player management features.

### CORS Configuration

The application uses two methods to handle CORS:

1. **Angular Proxy**: A proxy configuration in `proxy.conf.json` redirects API requests to avoid CORS issues during development
2. **HttpClient Configuration**: The Angular HttpClient is configured with credentials support

If you're still experiencing CORS issues, ensure your API server has proper CORS headers configured.

## Project Structure

- `/src/app/components/main-form` - Main form component that mimics the WinForms MainForm
- `/src/app/components/player-list` - Player management interface
- `/src/app/components/player-conflicts` - Player conflicts management
- `/src/app/services/api` - Service for REST API communication
- `/src/app/models/player` - TypeScript interfaces for player data

## Implemented Features

### Players Menu (Fully Functional)
- **Manage...**: Complete player CRUD operations
  - Add new players with validation
  - Edit existing players
  - Delete players (with game dependency checks)
  - Sort by first/last name
  - Email address validation
- **Conflicts...**: Player conflict management
  - View existing conflicts
  - Add new conflicts with values 0-10
  - Update/remove conflicts

### Stubbed Features
- Group menu items
- Scoring menu items
- Event menu items
- Configuration menu items
- Help menu items
- Main form action buttons

## Notes

- The application uses Angular 19 with zoneless change detection
- Styling mimics the WinForms appearance
- All API calls include proper error handling
- Form validation matches the original WinForms behavior
