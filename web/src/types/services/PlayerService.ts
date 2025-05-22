import { Player } from '../../models/Player';
import { Group } from '../../models/Group';
import { Game } from '../../models/Game';

// Interface for player conflict
export interface PlayerConflict {
  player: Player;
  value: number;
}

export interface PlayerService {
  getAll(): Promise<Player[]>;
  getById(id: number): Promise<Player>;
  create(player: Player): Promise<Player>;
  update(id: number, player: Partial<Player>): Promise<Player>;
  delete(id: number): Promise<void>;
  normalizeName(player: Player): Promise<string>;
  getPlayerGames(id: number): Promise<Game[]>;
  getPlayerGroups(id: number): Promise<Group[]>;
  getPlayerConflicts(id: number): Promise<PlayerConflict[]>;
  updatePlayerConflict(
    id: number,
    playerId: number,
    value?: number
  ): Promise<PlayerConflict>;
}
