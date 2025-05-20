// types/services/TournamentService.ts
import { Round, Tournament } from '../../models/Tournament';
import { Game } from '../../models/Game';

export interface RoundInfo {
  number: number;
  name: string;
  boardCount: number;
  availableBoards: number[];
}

export interface GamePlacementValidation {
  isValid: boolean;
  message?: string;
  conflicts?: {
    type: 'round' | 'board' | 'player';
    message: string;
  }[];
}

export interface TournamentService {
  getAll(): Promise<Tournament[]>;
  getById(id: number): Promise<Tournament>;
  create(tournament: Tournament): Promise<Tournament>;
  update(id: number, tournament: Partial<Tournament>): Promise<Tournament>;
  delete(id: number): Promise<void>;
  getRounds(tournamentId: number): Promise<RoundInfo[]>;
  getAvailableBoards(tournamentId: number, round: number): Promise<number[]>;
  getNextAvailableBoard(
    tournamentId: number,
    round: number
  ): Promise<number | null>;
  validateBoard(
    tournamentId: number,
    round: number,
    board: number,
    game: Game
  ): Promise<boolean>;
  getActiveTournaments(): Promise<Tournament[]>;
}
