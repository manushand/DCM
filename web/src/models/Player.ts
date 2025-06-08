import { DetailedModel } from './BaseModel';

export interface Player extends DetailedModel {
  firstName?: string;
  lastName?: string;
  emailAddresses?: string[];
  score?: number;
  rating?: number;
}

export interface PlayerDetails {
  emailAddresses: string[];
  // Add more properties as needed based on the API
}
