import { CrudService } from './crudService';
import { Player } from '../models/Player';
import { Game } from '../models/Game';
import { Group } from '../models/Group';
import api from './api';

// Interface for player conflict
export interface PlayerConflict {
  player: Player;
  value: number;
}

class PlayerService extends CrudService<Player> {
  constructor() {
    super('player');
  }

  // Get all games for a player
  async getPlayerGames(id: number): Promise<Game[]> {
    const response = await api.get<Game[]>(`player/${id}/games`);
    return response.data;
  }

  // Get all groups a player belongs to
  async getPlayerGroups(id: number): Promise<Group[]> {
    const response = await api.get<Group[]>(`player/${id}/groups`);
    return response.data;
  }

  // Get all conflicts for a player
  async getPlayerConflicts(id: number): Promise<PlayerConflict[]> {
    const response = await api.get<PlayerConflict[]>(`player/${id}/conflicts`);
    return response.data;
  }

  // Get or update a player conflict
  async updatePlayerConflict(id: number, playerId: number, value?: number): Promise<PlayerConflict> {
    const response = await api.patch<PlayerConflict>(`player/${id}/conflict/${playerId}`, { value });
    return response.data;
  }
}

export const playerService = new PlayerService();
