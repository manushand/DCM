import { CrudService } from './crudService';
import { Tournament, Round } from '../models/Tournament';
import { Game } from '../models/Game';
import api from './api';

interface RoundInfo {
  number: number;
  name: string;
  boardCount: number;
  availableBoards: number[];
}

interface GamePlacementValidation {
  isValid: boolean;
  message?: string;
  conflicts?: {
    type: 'round' | 'board' | 'player';
    message: string;
  }[];
}

class TournamentService extends CrudService<Tournament> {
  constructor() {
    super('tournament');
  }

  /**
   * Get all active tournaments (not completed)
   */
  async getActiveTournaments(): Promise<Tournament[]> {
    try {
      const response = await api.get(`${this.baseUrl}/active`);
      return response.data;
    } catch (error) {
      console.error('Error fetching active tournaments:', error);
      throw new Error('Failed to fetch active tournaments');
    }
  }

  /**
   * Get a tournament with detailed round information
   */
  async getTournamentWithRounds(tournamentId: number): Promise<Tournament> {
    try {
      const response = await api.get(`${this.baseUrl}/${tournamentId}/rounds`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching tournament ${tournamentId} with rounds:`, error);
      throw new Error('Failed to fetch tournament rounds');
    }
  }

  /**
   * Get detailed information about rounds in a tournament
   */
  async getRounds(tournamentId: number): Promise<RoundInfo[]> {
    try {
      const response = await api.get(`${this.baseUrl}/${tournamentId}/rounds/info`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching rounds for tournament ${tournamentId}:`, error);
      throw new Error('Failed to fetch tournament rounds');
    }
  }

  /**
   * Get available boards for a specific round in a tournament
   */
  async getAvailableBoards(tournamentId: number, roundNumber: number): Promise<number[]> {
    try {
      const response = await api.get(`${this.baseUrl}/${tournamentId}/rounds/${roundNumber}/boards/available`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching available boards for tournament ${tournamentId}, round ${roundNumber}:`, error);
      throw new Error('Failed to fetch available boards');
    }
  }

  /**
   * Get games in a specific round of a tournament
   */
  async getRoundGames(tournamentId: number, roundNumber: number): Promise<Game[]> {
    try {
      const response = await api.get(`${this.baseUrl}/${tournamentId}/rounds/${roundNumber}/games`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching games for tournament ${tournamentId}, round ${roundNumber}:`, error);
      throw new Error('Failed to fetch round games');
    }
  }

  /**
   * Validate if a game can be placed in the specified tournament, round, and board
   */
  async validateGamePlacement(
    tournamentId: number, 
    roundNumber: number, 
    boardNumber: number, 
    game: Game
  ): Promise<GamePlacementValidation> {
    try {
      const response = await api.post(
        `${this.baseUrl}/${tournamentId}/rounds/${roundNumber}/boards/${boardNumber}/validate`, 
        game
      );
      return response.data;
    } catch (error) {
      console.error('Error validating game placement:', error);
      return {
        isValid: false,
        message: 'Failed to validate game placement'
      };
    }
  }

  /**
   * Get the next available board number for a specific round in a tournament
   */
  async getNextAvailableBoard(tournamentId: number, roundNumber: number): Promise<number | null> {
    try {
      const availableBoards = await this.getAvailableBoards(tournamentId, roundNumber);
      return availableBoards.length > 0 ? Math.min(...availableBoards) : null;
    } catch (error) {
      console.error('Error getting next available board:', error);
      return null;
    }
  }

  /**
   * Check if a tournament has started
   */
  async isTournamentStarted(tournamentId: number): Promise<boolean> {
    try {
      const tournament = await this.getById(tournamentId);
      return tournament.rounds && tournament.rounds.some(r => 
        r.games && r.games.some(g => g.status !== 'Scheduled')
      );
    } catch (error) {
      console.error('Error checking if tournament has started:', error);
      return false;
    }
  }
}

export const tournamentService = new TournamentService();
