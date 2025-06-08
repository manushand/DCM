import type { GameService } from '../types/services/GameService';
import {
  Game,
  GamePlayer,
  GameResult,
  GameStatus,
  Powers,
} from '../models/Game';

export class MockGameService implements GameService {
  private games: Game[] = [
    {
      id: 1,
      name: 'Game 1',
      status: GameStatus.Scheduled,
      tournamentId: 100,
      tournamentName: 'Mock Tournament',
      round: 1,
      board: 1,
      players: [
        {
          playerId: 1,
          playerName: 'Alice',
          power: Powers.England,
          result: GameResult.Unknown,
          playComplete: false,
        },
        {
          playerId: 2,
          playerName: 'Bob',
          power: Powers.France,
          result: GameResult.Unknown,
          playComplete: false,
        },
      ],
      scoringSystemId: 1,
      scoringSystemIsDefault: true,
    },
    {
      id: 2,
      name: 'Game 2',
      status: GameStatus.Underway,
      tournamentId: 100,
      tournamentName: 'Mock Tournament',
      round: 1,
      board: 2,
      players: [
        {
          playerId: 1,
          playerName: 'Alice',
          power: Powers.England,
          result: GameResult.Unknown,
          playComplete: false,
        },
        {
          playerId: 2,
          playerName: 'Bob',
          power: Powers.France,
          result: GameResult.Unknown,
          playComplete: false,
        },
      ],
      scoringSystemId: 1,
      scoringSystemIsDefault: true,
    },
  ];

  getAll(): Promise<Game[]> {
    return Promise.resolve(this.games);
  }

  getById(id: number): Promise<Game> {
    const game = this.games.find((g) => g.id === id);
    return game
      ? Promise.resolve(game)
      : Promise.reject(new Error('Game not found'));
  }

  create(game: Game): Promise<Game> {
    const newGame = { ...game, id: Date.now() };
    this.games.push(newGame);
    return Promise.resolve(newGame);
  }

  update(id: number, update: Partial<Game>): Promise<Game> {
    const index = this.games.findIndex((g) => g.id === id);
    if (index === -1) return Promise.reject(new Error('Game not found'));

    this.games[index] = { ...this.games[index], ...update };
    return Promise.resolve(this.games[index]);
  }

  delete(id: number): Promise<void> {
    this.games = this.games.filter((g) => g.id !== id);
    return Promise.resolve();
  }

  validate(game: Game): Promise<boolean> {
    // Pretend all games are valid in the mock
    return Promise.resolve(true);
  }

  calculateConflicts(gamePlayer: GamePlayer, game: Game): number {
    return 0; // No conflicts in the mock
  }
}
