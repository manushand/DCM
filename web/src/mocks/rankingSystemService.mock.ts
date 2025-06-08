import type { RankingSystemService } from '../types/services/RankingSystemService';
import type { RankingSystem } from '../models/RankingSystem';

export class MockRankingSystemService implements RankingSystemService {
  private rankingSystems: RankingSystem[] = [
    { id: 1, name: 'Standard Ranking', isDefault: true },
    { id: 2, name: 'Modified Ranking', isDefault: false },
  ];

  getAll(): Promise<RankingSystem[]> {
    return Promise.resolve(this.rankingSystems);
  }

  getById(id: number): Promise<RankingSystem> {
    const found = this.rankingSystems.find((s) => s.id === id);
    return found
      ? Promise.resolve(found)
      : Promise.reject(new Error('Ranking system not found'));
  }

  getDefault(): Promise<RankingSystem> {
    const defaultSystem = this.rankingSystems.find((s) => s.isDefault);
    return defaultSystem
      ? Promise.resolve(defaultSystem)
      : Promise.reject(new Error('No default ranking system found'));
  }
}
