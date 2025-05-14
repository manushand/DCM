import { CrudService } from './crudService';
import { Group } from '../models/Group';
import { Game } from '../models/Game';
import api from './api';

class GroupService extends CrudService<Group> {
  constructor() {
    super('group');
  }

  // Get all games for a group
  async getGroupGames(id: number): Promise<Game[]> {
    const response = await api.get<Game[]>(`group/${id}/games`);
    return response.data;
  }
}

export const groupService = new GroupService();
