import { Game, GameResult, GameStatus, Powers } from '../models/Game';
import { Group } from '../models/Group';
import { Player } from '../models/Player';
import type {
  PlayerConflict,
  PlayerService,
} from '../types/services/PlayerService';

export class MockPlayerService implements PlayerService {
  private playerGames: Game[] = [
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
  private players: Player[] = [
    {
      id: 1,
      name: 'Alice Smith',
      firstName: 'Alice',
      lastName: 'Smith',
      emailAddresses: ['asmith@example.com'],
      rating: 1500,
    },
    {
      id: 2,
      name: 'Bob Smith',
      firstName: 'Bob',
      lastName: 'Smith',
      emailAddresses: ['bsmith@example.com'],
      rating: 1450,
    },
  ];

  getPlayerGames(id: number): Promise<Game[]> {
    const games = this.playerGames.filter((game) =>
      game.players?.some((p) => p.playerId === id)
    );
    return Promise.resolve(games);
  }
  getPlayerGroups(id: number): Promise<Group[]> {
    throw new Error('Method not implemented.');
  }
  getPlayerConflicts(id: number): Promise<PlayerConflict[]> {
    throw new Error('Method not implemented.');
  }
  updatePlayerConflict(
    id: number,
    playerId: number,
    value?: number | undefined
  ): Promise<PlayerConflict> {
    throw new Error('Method not implemented.');
  }

  getAll(): Promise<Player[]> {
    return Promise.resolve(this.players);
  }

  getById(id: number): Promise<Player> {
    const player = this.players.find((p) => p.id === id);
    return player
      ? Promise.resolve(player)
      : Promise.reject(new Error('Player not found'));
  }

  create(player: Player): Promise<Player> {
    const newPlayer = { ...player, id: Date.now() };
    this.players.push(newPlayer);
    return Promise.resolve(newPlayer);
  }

  update(id: number, player: Partial<Player>): Promise<Player> {
    const index = this.players.findIndex((p) => p.id === id);
    if (index === -1) return Promise.reject(new Error('Player not found'));

    this.players[index] = { ...this.players[index], ...player };
    return Promise.resolve(this.players[index]);
  }

  delete(id: number): Promise<void> {
    this.players = this.players.filter((p) => p.id !== id);
    return Promise.resolve();
  }
}
