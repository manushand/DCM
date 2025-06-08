import { RankingSystem } from '../../models/RankingSystem';

export interface RankingSystemService {
  getAll(): Promise<RankingSystem[]>;
  getById(id: number): Promise<RankingSystem>;
  getDefault(): Promise<RankingSystem>;
}
