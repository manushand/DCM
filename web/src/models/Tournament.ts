import { DetailedModel } from './BaseModel';
import { Game } from './Game';

export interface Round {
  number: number;
  games?: Game[];
}

export enum TournamentStatus {
  Scheduled = 'Scheduled',
  Cancelled = 'Cancelled',
  Underway = 'Underway', // Changed to match WinForms
  Finished = 'Finished', // Changed to match WinForms
}

export interface Tournament extends DetailedModel {
  status: TournamentStatus;
  rounds?: Round[];
  isEvent?: boolean;
}

export interface TournamentDetails {
  rounds: Round[];
  isEvent: boolean;
  // Add more properties as needed based on the API
}
