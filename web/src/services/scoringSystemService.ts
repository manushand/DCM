import { CrudService } from './crudService';
import { ScoringSystem } from '../models/Game';

class ScoringSystemService extends CrudService<ScoringSystem> {
  constructor() {
    super('scoringSystem');
  }
  
  async getDefault(): Promise<ScoringSystem> {
    const response = await this.getAll();
    const defaultSystem = response.find(system => system.isDefault);
    
    if (!defaultSystem) {
      // Return a reasonable default if no default is found
      return {
        id: 0,
        name: 'Standard Scoring',
        isDefault: true,
        description: 'Standard tournament scoring system',
        drawCalculation: 'Equal Share',
        eliminationValue: 0,
        survivorValue: 1,
        soloValue: 100,
        centerValue: 1,
        yearValue: 0.1
      };
    }
    
    return defaultSystem;
  }
}

export const scoringSystemService = new ScoringSystemService();

