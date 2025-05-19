import { DetailedModel } from './BaseModel';

export enum Powers {
  None = 'None',
  Admin = 'Admin',
  Observer = 'Observer',
  Austria = 'Austria',
  England = 'England',
  France = 'France',
  Germany = 'Germany',
  Italy = 'Italy',
  Russia = 'Russia',
  Turkey = 'Turkey'
}

export enum GameStatus {
  Scheduled = 'Scheduled',
  Cancelled = 'Cancelled',
  Seeded = 'Seeded', // Changed to match WinForms
  Underway = 'Underway', // Changed to match WinForms
  Finished = 'Finished' // Changed to match WinForms
}

export enum GameResult {
  Unknown = 'Unknown',
  Win = 'Win',
  Draw = 'Draw',
  Loss = 'Loss',
  Eliminated = 'Eliminated'
}

export interface PlayerConflict {
  playerId: number;
  reason: string;
  severity: number;
}

export interface GamePlayer {
  playerId: number;
  playerName: string;
  power: Powers;
  result: GameResult;
  score?: number;
  centers?: number; // Number of supply centers
  years?: number; // Years survived
  playComplete: boolean; // Indicates if player data is complete
  conflict?: number; // Conflict score
  conflictDetails?: PlayerConflict[]; // Detailed conflict information
}

export type GamePlayers = GamePlayer[];

export interface Game extends DetailedModel {
  status: GameStatus;
  tournamentId?: number;
  tournamentName?: string;
  round?: number;
  board?: number;
  players?: GamePlayers;
  scoringSystemId?: number;
  scoringSystem?: ScoringSystem;
  scoringSystemIsDefault?: boolean;
}

export interface ScoringSystem extends DetailedModel {
  description?: string;
  drawCalculation?: string;
  eliminationValue?: number;
  survivorValue?: number;
  soloValue?: number;
  centerValue?: number;
  yearValue?: number;
  isDefault?: boolean;
}

export interface GameDetails {
  status: GameStatus;
  tournamentId: number;
  tournamentName: string;
  round: number;
  board: number;
  players: GamePlayer[];
  scoringSystemId: number;
  scoringSystem: ScoringSystem;
  // Add more properties as needed based on the API
}

// Helper function to check if all powers are assigned
export const allPowersAssigned = (game: Game): boolean => {
  if (!game.players || game.players.length === 0) return false;
  return !game.players.some(p => p.power === Powers.None);
};

// Helper function to check if game data is complete
export const isGameDataComplete = (game: Game): boolean => {
  if (!game.players || game.players.length === 0) return false;
  
  // All players must have powers assigned
  if (!allPowersAssigned(game)) return false;
  
  // If the game is Finished, all players must have complete data
  if (game.status === GameStatus.Finished) {
    return game.players.every(p => (
      p.power !== Powers.None &&
      p.result !== GameResult.Unknown &&
      p.centers !== undefined &&
      p.years !== undefined
    ));
  }
  
  return true;
};
