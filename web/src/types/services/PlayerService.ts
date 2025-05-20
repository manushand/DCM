import { Player } from '../../models/Player';

export interface PlayerService {
  getAll(): Promise<Player[]>;
  getById(id: number): Promise<Player>;
  create(player: Player): Promise<Player>;
  update(id: number, player: Partial<Player>): Promise<Player>;
  delete(id: number): Promise<void>;
}
