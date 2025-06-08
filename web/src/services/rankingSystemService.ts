import { CrudService } from './crudService';
import { RankingSystem } from '../models/RankingSystem';
import { RankingSystemService } from '../types/services/RankingSystemService';

export class ApiRankingSystemService
  extends CrudService<RankingSystem>
  implements RankingSystemService
{
  constructor() {
    super('rankingSystem');
  }

  async getDefault(): Promise<RankingSystem> {
    const response = await this.getAll();
    const defaultSystem = response.find((system) => system.isDefault);

    if (!defaultSystem) {
      // Return a reasonable default if no default is found
      return {
        id: 0,
        name: 'Standard Ranking',
        isDefault: true,
        description: 'Standard ranking system',
      };
    }

    return defaultSystem;
  }
}
