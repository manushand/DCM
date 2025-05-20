import type { ScoringSystemService } from '../types/services/ScoringSystemService';
import type { ScoringSystem } from '../models/ScoringSystem';

export class MockScoringSystemService implements ScoringSystemService {
  private scoringSystems: ScoringSystem[] = [
    { id: 1, name: 'Standard Swiss', isDefault: true },
    { id: 2, name: 'Modified Swiss', isDefault: false },
  ];

  getAll(): Promise<ScoringSystem[]> {
    return Promise.resolve(this.scoringSystems);
  }

  getById(id: number): Promise<ScoringSystem> {
    const found = this.scoringSystems.find((s) => s.id === id);
    return found
      ? Promise.resolve(found)
      : Promise.reject(new Error('Scoring system not found'));
  }

  getDefault(): Promise<ScoringSystem> {
    const defaultSystem = this.scoringSystems.find((s) => s.isDefault);
    return defaultSystem
      ? Promise.resolve(defaultSystem)
      : Promise.reject(new Error('No default scoring system found'));
  }
}
