import { DetailedModel } from './BaseModel';

export interface GroupMember {
  playerId: number;
  playerName: string;
  rating?: number;
}

export interface Group extends DetailedModel {
  members?: GroupMember[];
}

export interface GroupDetails {
  members: GroupMember[];
  // Add more properties as needed based on the API
}
