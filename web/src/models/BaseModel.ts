export interface BaseModel {
  id: number;
  name: string;
}

export interface DetailedModel extends BaseModel {
  details?: any;
}