import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api';
import { Player, PlayerConflict } from '../../models/player';

@Component({
  selector: 'app-player-conflicts',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './player-conflicts.html',
  styleUrl: './player-conflicts.css'
})
export class PlayerConflictsComponent implements OnInit {
  player: Player | null = null;
  allPlayers: Player[] = [];
  conflicts: PlayerConflict[] = [];
  selectedPlayerId: number | null = null;
  conflictValue: number = 5; // Default middle value
  loading = false;

  constructor(
    private apiService: ApiService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadAllPlayers();

    this.route.paramMap.subscribe(params => {
      const playerId = params.get('id');
      if (playerId) {
        this.loadPlayer(+playerId);
        this.loadConflicts(+playerId);
      }
    });
  }

  loadAllPlayers() {
    this.apiService.getPlayers().subscribe({
      next: (players) => {
        this.allPlayers = players;
      },
      error: (error) => {
        console.error('Error loading players:', error);
      }
    });
  }

  loadPlayer(id: number) {
    this.apiService.getPlayer(id).subscribe({
      next: (player) => {
        this.player = player;
      },
      error: (error) => {
        console.error('Error loading player:', error);
        alert('Error loading player details.');
        this.goBack();
      }
    });
  }

  loadConflicts(id: number) {
    this.loading = true;
    this.apiService.getPlayerConflicts(id).subscribe({
      next: (conflicts) => {
        this.conflicts = conflicts;
        this.loading = false;
      },
      error: (error) => {
        console.error('Error loading conflicts:', error);
        alert('Error loading player conflicts.');
        this.loading = false;
      }
    });
  }

  get availablePlayers(): Player[] {
    if (!this.player) return [];

    // Filter out the current player and any players that already have conflicts
    return this.allPlayers.filter(p =>
      p.id !== this.player!.id &&
      !this.conflicts.some(c => c.conflictPlayerId === p.id)
    );
  }

  addConflict() {
    if (!this.player || !this.selectedPlayerId) return;

    this.apiService.setPlayerConflict(
      this.player.id,
      this.selectedPlayerId,
      this.conflictValue
    ).subscribe({
      next: (conflict) => {
        this.conflicts.push(conflict);
        this.selectedPlayerId = null;
        this.conflictValue = 5;
      },
      error: (error) => {
        console.error('Error adding conflict:', error);
        alert('Error adding player conflict.');
      }
    });
  }

  updateConflict(conflict: PlayerConflict) {
    if (!this.player) return;

    this.apiService.setPlayerConflict(
      this.player.id,
      conflict.conflictPlayerId,
      conflict.value
    ).subscribe({
      next: () => {
        // Successfully updated
      },
      error: (error) => {
        console.error('Error updating conflict:', error);
        alert('Error updating player conflict.');
        // Reload conflicts to reset UI
        this.loadConflicts(this.player!.id);
      }
    });
  }

  removeConflict(conflict: PlayerConflict) {
    if (!this.player) return;

    if (confirm(`Remove conflict with ${conflict.conflictPlayerName}?`)) {
      // Setting value to undefined removes the conflict
      this.apiService.setPlayerConflict(
        this.player.id,
        conflict.conflictPlayerId
      ).subscribe({
        next: () => {
          this.conflicts = this.conflicts.filter(
            c => c.conflictPlayerId !== conflict.conflictPlayerId
          );
        },
        error: (error) => {
          console.error('Error removing conflict:', error);
          alert('Error removing player conflict.');
        }
      });
    }
  }

  goBack() {
    if (this.player) {
      this.router.navigate(['/players']);
    } else {
      this.router.navigate(['/']);
    }
  }
}
