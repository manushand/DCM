export interface Player {
  id: number;
  name: string;
  firstName: string;
  lastName: string;
  details?: {
    emailAddresses?: string[];
    [key: string]: any;
  };
  // Add a timestamp to force view refresh
  _lastUpdated?: number;
}

export interface PlayerDetails {
  emailAddresses?: string[];
  [key: string]: any;
}

export interface PlayerConflict {
  id?: number;
  playerId: number;
  playerName?: string;
  conflictPlayerId: number;
  conflictPlayerName: string;
  value: number;
}
