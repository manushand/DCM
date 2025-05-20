import type { GroupService } from '../types/services/GroupService';
import type { Group } from '../models/Group';
import {Game} from "../models/Game";

export class MockGroupService implements GroupService {
  private groups: Group[] = [
    { id: 1, name: 'Group Alpha' },
    { id: 2, name: 'Group Beta' },
  ];

  getAll(): Promise<Group[]> {
    return Promise.resolve(this.groups);
  }

  getById(id: number): Promise<Group> {
    const group = this.groups.find((g) => g.id === id);
    return group
      ? Promise.resolve(group)
      : Promise.reject(new Error('Group not found'));
  }

  create(group: Group): Promise<Group> {
    const newGroup = { ...group, id: Date.now() };
    this.groups.push(newGroup);
    return Promise.resolve(newGroup);
  }

  update(id: number, data: Partial<Group>): Promise<Group> {
    const index = this.groups.findIndex((g) => g.id === id);
    if (index === -1) return Promise.reject(new Error('Group not found'));

    this.groups[index] = { ...this.groups[index], ...data };
    return Promise.resolve(this.groups[index]);
  }

  delete(id: number): Promise<void> {
    this.groups = this.groups.filter((g) => g.id !== id);
    return Promise.resolve();
  }

  getGroupGames(id: number): Promise<Game[]>{
    return Promise.resolve([]);
  }
}
