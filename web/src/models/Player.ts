import { DetailedModel } from './BaseModel';

export interface Player extends DetailedModel {
  firstName?: string;
  lastName?: string;
  emailAddresses?: string[];
}

export interface PlayerDetails {
  emailAddresses: string[];
  // Add more properties as needed based on the API
}
