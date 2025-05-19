import { CrudService } from './crudService';
import { Game, GamePlayer, calculateGameScores } from '../models/Game';

class GameService extends CrudService<Game> {
  constructor() {
    super('game');
  }

  // Calculate conflicts for a game player
  calculateConflicts(gamePlayer: GamePlayer, game: Game): number {
    if (!game.players || game.tournamentId === undefined) {
      return 0;
    }

    // This would typically call an API endpoint or use local data
    // to calculate conflicts based on player history and tournament policies
    // For now, we'll implement a simplified version
    const conflicts: PlayerConflict[] = [];
    let totalConflict = 0;

    // Check for players from the same country/city
    const otherPlayers = game.players.filter(
      (p) => p.playerId !== gamePlayer.playerId
    );

    // This is simplified - in a real implementation we would check actual conflicts
    // such as players who shouldn't play together based on tournament rules
    for (const other of otherPlayers) {
      if (this.playersHaveConflict(gamePlayer.playerId, other.playerId)) {
        conflicts.push({
          playerId: other.playerId,
          reason: `Previous conflict with ${other.playerName}`,
          severity: 1,
        });
        totalConflict += 1;
      }
    }

    // Store conflicts in the player object
    gamePlayer.conflict = totalConflict;
    gamePlayer.conflictDetails = conflicts;

    return totalConflict;
  }

  // Check if two players have a conflict
  // In a real implementation, this would be based on actual data
  playersHaveConflict(player1Id: number, player2Id: number): boolean {
    // This is a placeholder - in reality we would check a database of
    // known conflicts, player preferences, etc.
    // For demo purposes, we'll just generate conflicts randomly
    return Math.random() < 0.2; // 20% chance of conflict
  }

  // Calculate and update game scores
  calculateScores(game: Game): boolean {
    return calculateGameScores(game);
  }
}

export const gameService = new GameService();
