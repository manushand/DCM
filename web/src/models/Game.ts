import { DetailedModel } from './BaseModel';

export enum Powers {
  TBD = 'TBD',
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

export interface Game extends DetailedModel {
  status: GameStatus;
  tournamentId?: number;
  tournamentName?: string;
  round?: number;
  board?: number;
  players?: GamePlayer[];
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

// Game score calculation helpers
export const calculateGameScores = (game: Game): boolean => {
  if (!game.players || !game.scoringSystem) return false;
  
  // Total supply centers and players with centers
  const totalCenters = game.players.reduce((sum, p) => sum + (p.centers || 0), 0);
  const playersWithCenters = game.players.filter(p => p.centers && p.centers > 0);
  
  // Calculate scores based on the scoring system
  game.players.forEach(player => {
    if (!player.playComplete) return;
    
    let score = 0;
    const system = game.scoringSystem!;
    
    switch (player.result) {
      case GameResult.Win:
        score = system.soloValue || 100;
        break;
      case GameResult.Draw:
        const drawShare = 100 / playersWithCenters.length;
        score = drawShare * (player.centers || 0) / totalCenters;
        break;
      case GameResult.Loss:
        score = (player.centers || 0) * (system.centerValue || 1) +
               (player.years || 0) * (system.yearValue || 0.1);
        break;
      case GameResult.Eliminated:
        score = system.eliminationValue || 0;
        break;
    }
    
    player.score = Math.round(score * 100) / 100;
  });
  
  return true;
};

// Helper function to check if all powers are assigned
export const allPowersAssigned = (game: Game): boolean => {
  if (!game.players || game.players.length === 0) return false;
  return !game.players.some(p => p.power === Powers.TBD);
};

// Helper function to check if game data is complete
export const isGameDataComplete = (game: Game): boolean => {
  if (!game.players || game.players.length === 0) return false;
  
  // All players must have powers assigned
  if (!allPowersAssigned(game)) return false;
  
  // If the game is Finished, all players must have complete data
  if (game.status === GameStatus.Finished) {
    return game.players.every(p => (
      p.power !== Powers.TBD &&
      p.result !== GameResult.Unknown &&
      p.centers !== undefined &&
      p.years !== undefined
    ));
  }
  
  return true;
};
