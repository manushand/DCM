import { Group } from '../../models/Group';

export interface GroupService {
  getAll(): Promise<Group[]>;
  getById(id: number): Promise<Group>;
  create(group: Group): Promise<Group>;
  update(id: number, group: Partial<Group>): Promise<Group>;
  delete(id: number): Promise<void>;
}
