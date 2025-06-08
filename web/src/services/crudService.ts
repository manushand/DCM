import api from './api';
import { BaseModel } from '../models/BaseModel';

export class CrudService<T extends BaseModel> {
  protected endpoint: string;

  constructor(endpoint: string) {
    this.endpoint = endpoint;
  }

  async getAll(): Promise<T[]> {
    const response = await api.get<T[]>(`${this.endpoint}s`);
    return response.data;
  }

  async getById(id: number): Promise<T> {
    const response = await api.get<T>(`${this.endpoint}/${id}`);
    return response.data;
  }

  async create(item: Omit<T, 'id'>): Promise<T> {
    const response = await api.post(this.endpoint, item);
    return response.data;
  }

  async update(id: number, item: T): Promise<T> {
    const response = await api.put(`${this.endpoint}/${id}`, item);
    return response.data;
  }

  async delete(id: number): Promise<void> {
    await api.delete(`${this.endpoint}/${id}`);
  }

  // Add a generic method to get related items
  async getRelated<R>(id: number, relatedEndpoint: string): Promise<R[]> {
    const response = await api.get<R[]>(
      `${this.endpoint}/${id}/${relatedEndpoint}`
    );
    return response.data;
  }
}
