// mocks/MockTournamentService.ts
import type {
  RoundInfo,
  TournamentService,
} from '../types/services/TournamentService';
import { Tournament, TournamentStatus } from '../models/Tournament';
import type { Game } from '../models/Game';

export class MockTournamentService implements TournamentService {
  private mockTournaments: Tournament[] = [
    { id: 1, name: 'Mock Tournament A', status: TournamentStatus.Underway },
    { id: 2, name: 'Mock Tournament B', status: TournamentStatus.Finished },
    { id: 3, name: 'Mock Tournament C', status: TournamentStatus.Scheduled },
  ];

  getAll(): Promise<Tournament[]> {
    return Promise.resolve(this.mockTournaments);
  }

  getById(id: number): Promise<Tournament> {
    const match = this.mockTournaments.find((t) => t.id === id);
    return match
      ? Promise.resolve(match)
      : Promise.reject(new Error('Not found'));
  }

  create(tournament: Tournament): Promise<Tournament> {
    const newTournament = { ...tournament, id: Date.now() };
    this.mockTournaments.push(newTournament);
    return Promise.resolve(newTournament);
  }

  update(id: number, update: Partial<Tournament>): Promise<Tournament> {
    const index = this.mockTournaments.findIndex((t) => t.id === id);
    if (index === -1) return Promise.reject(new Error('Not found'));

    this.mockTournaments[index] = { ...this.mockTournaments[index], ...update };
    return Promise.resolve(this.mockTournaments[index]);
  }

  delete(id: number): Promise<void> {
    this.mockTournaments = this.mockTournaments.filter((t) => t.id !== id);
    return Promise.resolve();
  }

  getRounds(tournamentId: number): Promise<RoundInfo[]> {
    // Simulate 3 rounds
    return Promise.resolve([
      {
        number: 1,
        name: 'Round 1',
        boardCount: 4,
        availableBoards: [1, 2, 3, 4],
      },
      { number: 2, name: 'Round 2', boardCount: 2, availableBoards: [1, 2] },
      { number: 3, name: 'Round 3', boardCount: 3, availableBoards: [1, 2, 3] },
    ]);
  }

  getAvailableBoards(tournamentId: number, round: number): Promise<number[]> {
    return Promise.resolve([1, 2, 3]); // Simulated boards
  }

  getNextAvailableBoard(
    tournamentId: number,
    round: number
  ): Promise<number | null> {
    return Promise.resolve(1); // Always return 1 as "next available"
  }

  validateBoard(
    tournamentId: number,
    round: number,
    board: number,
    game: Game
  ): Promise<boolean> {
    return Promise.resolve(true); // Always valid
  }

  getActiveTournaments(): Promise<Tournament[]> {
    return Promise.resolve(
      this.mockTournaments.filter((t) => t.status === TournamentStatus.Underway)
    );
  }
}
