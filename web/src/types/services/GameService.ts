import { Game } from '../../models/Game';

export interface GameService {
  getAll(): Promise<Game[]>;
  getById(id: number): Promise<Game>;
  create(game: Game): Promise<Game>;
  update(id: number, game: Partial<Game>): Promise<Game>;
  delete(id: number): Promise<void>;
}
