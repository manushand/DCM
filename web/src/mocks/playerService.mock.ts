import { Player } from '../models/Player';
import type { PlayerService } from '../types/services/PlayerService';

export class MockPlayerService implements PlayerService {
  private players: Player[] = [
    { id: 1, name: 'Alice', rating: 1500 },
    { id: 2, name: 'Bob', rating: 1450 },
  ];

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
