import { GameResult, GameStatus } from '../models/Game';

// Function to get color for status chip
export const getStatusColor = (status: GameStatus) => {
  switch (status) {
    case GameStatus.Scheduled:
      return 'primary';
    case GameStatus.Underway:
      return 'warning';
    case GameStatus.Finished:
      return 'success';
    case GameStatus.Cancelled:
      return 'error';
    default:
      return 'default';
  }
};

// Function to get color for result chip
export const getResultColor = (result: GameResult) => {
  switch (result) {
    case GameResult.Win:
      return 'success';
    case GameResult.Draw:
      return 'info';
    case GameResult.Loss:
      return 'error';
    default:
      return 'default';
  }
};
