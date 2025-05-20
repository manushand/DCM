import { ScoringSystem } from '../../models/ScoringSystem';

export interface ScoringSystemService {
  getAll(): Promise<ScoringSystem[]>;
  getById(id: number): Promise<ScoringSystem>;
  getDefault(): Promise<ScoringSystem>;
}
