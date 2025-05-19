import { DetailedModel } from './BaseModel';
import { Game } from './Game';

export interface Round {
  number: number;
  games?: Game[];
}

export interface Tournament extends DetailedModel {
  rounds?: Round[];
  isEvent?: boolean;
}

export interface TournamentDetails {
  rounds: Round[];
  isEvent: boolean;
  // Add more properties as needed based on the API
}
