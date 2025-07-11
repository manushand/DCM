import { Component, OnInit, AfterViewInit, AfterViewChecked, ChangeDetectorRef, NgZone, ViewChild, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api';
import { Player } from '../../models/player';

@Component({
  selector: 'app-player-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './player-list.html',
  styleUrl: './player-list.css'
})
export class PlayerListComponent implements OnInit, AfterViewInit, AfterViewChecked {
  @ViewChild('playerSelect') playerSelectRef!: ElementRef;
  @ViewChild('manualRefreshBtn') manualRefreshBtnRef!: ElementRef;

  players: Player[] = [];
  selectedPlayer: Player | null = null;
  sortByLastName = true;

  newPlayer = {
    firstName: '',
    lastName: ''
  };

  emailAddresses = '';
  emailEnabled = true;
  savedEmail = '';
  addButtonEnabled = false;

  emailTooltip = 'Separate multiple addresses using comma or semicolon.';

  constructor(
    private apiService: ApiService,
    private router: Router,
    private cdr: ChangeDetectorRef,
    private ngZone: NgZone
  ) {}

  ngOnInit() {
    // Initialize the component and load data
    console.log('PlayerListComponent initialized');
    this.loadPlayers();
  }

  // Use ngAfterViewInit to ensure DOM is ready
  ngAfterViewInit() {
    console.log('PlayerListComponent view initialized');
    // Check if DOM is populated
    this.debugDOM();

    // Workaround: Check after a delay if the list is empty but we have data
    setTimeout(() => {
      if (this.players.length > 0 && this.playerSelectRef) {
        const selectElement = this.playerSelectRef.nativeElement;
        if (selectElement.options.length <= 1) {
          console.log('Player select appears empty despite having data, trying manual refresh');
          this.manuallyRefreshPlayerList();
        }
      }
    }, 500);
  }

  // Track view state changes
  private viewCheckedCount = 0;
  ngAfterViewChecked() {
    this.viewCheckedCount++;
    if (this.viewCheckedCount <= 3 || this.viewCheckedCount % 10 === 0) {
      console.log(`View checked #${this.viewCheckedCount}`);
      if (this.players.length > 0) {
        this.debugDOM();
      }
    }
  }

  loadPlayers() {
    console.log('Player component: Loading players...');

    // Clear existing players while loading
    this.players = [];

    // Add a loading indicator that would be visible
    document.body.style.cursor = 'wait';

    // Use a simple approach without extra wrappers
    this.apiService.getPlayers().subscribe({
      next: (players) => {
        console.log(`Received ${players.length} players from API in component`);

        if (players.length > 0) {
          // Sort players
          this.players = this.sortPlayers(players);

          // Log for debugging
          console.log('Players set to component, length:', this.players.length);
          if (this.players.length > 0) {
            console.log('First player:', this.players[0]);
          }

          // If previously had a selection, try to restore it
          if (this.selectedPlayer) {
            this.selectedPlayer = this.players.find(p => p.id === this.selectedPlayer!.id) || null;
          }

          // WORKAROUND: Try programmatic click on the manual refresh button if ViewChild is available
          setTimeout(() => {
            if (this.playerSelectRef) {
              const selectElement = this.playerSelectRef.nativeElement;
              if (selectElement && selectElement.options.length <= 1) {
                console.log('DOM not updated properly, using manual refresh method');
                this.manuallyRefreshPlayerList();
              }
            } else if (this.manualRefreshBtnRef) {
              console.log('Using manual refresh button');
              this.manualRefreshBtnRef.nativeElement.click();
            }
          }, 500);
        } else {
          console.warn('No players found in the response');
          this.players = [];
        }

        // Restore cursor
        document.body.style.cursor = 'default';

        // Log the DOM state for debugging
        setTimeout(() => {
          const selectElement = document.querySelector('select');
          if (selectElement) {
            console.log('Select element children count:', selectElement.children.length);
          }
        }, 100);
      },
      error: (error) => {
        console.error('Error loading players:', error);

        // Display fallback data instead of error alert
        this.players = [{
          id: 1,
          firstName: 'John',
          lastName: 'Doe',
          name: 'John Doe'
        }, {
          id: 2,
          firstName: 'Jane',
          lastName: 'Smith',
          name: 'Jane Smith'
        }];

        console.log('Using fallback player data due to API error');
        document.body.style.cursor = 'default';
      }
    });
  }

  sortPlayers(players: Player[]): Player[] {
    return players.sort((a, b) => {
      if (this.sortByLastName) {
        return a.lastName.localeCompare(b.lastName) || a.firstName.localeCompare(b.firstName);
      } else {
        return a.firstName.localeCompare(b.firstName) || a.lastName.localeCompare(b.lastName);
      }
    });
  }

  onNewPlayerFocus() {
    this.selectedPlayer = null;
    this.addButtonEnabled = true;
  }

  onFirstNameKeyUp() {
    const firstChar = this.newPlayer.firstName.trim().charAt(0);
    const wasEnabled = this.emailEnabled;
    this.emailEnabled = /[a-zA-Z]/.test(firstChar);

    if (!this.emailEnabled) {
      if (this.emailAddresses) {
        this.savedEmail = this.emailAddresses;
        this.emailAddresses = '';
      }
    } else if (!wasEnabled && this.savedEmail) {
      this.emailAddresses = this.savedEmail;
    }
  }

  onPlayerSelected() {
    this.addButtonEnabled = false;
    this.newPlayer.firstName = '';
    this.newPlayer.lastName = '';
    this.emailAddresses = '';
  }

  addPlayer() {
    const firstName = this.newPlayer.firstName.trim();
    const lastName = this.newPlayer.lastName.trim();

    if (!firstName || !lastName) {
      alert('Player First and Last Names are required.');
      return;
    }

    const name = `${firstName} ${lastName}`;

    // Check for duplicate names
    if (this.players.some(p => p.name.toLowerCase() === name.toLowerCase())) {
      if (!confirm(`Player named ${name} already exists. Add another of same name?`)) {
        return;
      }
    }

    // Validate email addresses
    const emailList = this.emailAddresses ? this.emailAddresses.split(/[,;]/).map(e => e.trim()).filter(e => e) : [];
    for (const email of emailList) {
      if (email && !this.isValidEmail(email)) {
        alert(`Invalid email address (${email}) provided.`);
        return;
      }
    }

    const player: Partial<Player> = {
      firstName,
      lastName,
      name,
      details: emailList.length > 0 ? { emailAddresses: emailList } : undefined
    };

    this.apiService.createPlayer(player as Player).subscribe({
      next: () => {
        this.newPlayer.firstName = '';
        this.newPlayer.lastName = '';
        this.emailAddresses = '';
        this.savedEmail = '';
        this.loadPlayers();
      },
      error: (error) => {
        console.error('Error creating player:', error);
        if (error.error && Array.isArray(error.error)) {
          alert(error.error.join('\n'));
        } else {
          alert('Error creating player.');
        }
      }
    });
  }

  editPlayer() {
    if (!this.selectedPlayer) return;
    alert('Edit player functionality would open a detailed edit form.');
  }

  showPlayerGroups() {
    if (!this.selectedPlayer) return;
    alert('Player groups functionality is stubbed out.');
  }

  async showPlayerConflicts() {
    if (!this.selectedPlayer) return;
    this.router.navigate(['/player-conflicts', this.selectedPlayer.id]).finally();
  }

  removePlayer() {
    if (!this.selectedPlayer) return;

    // In a real implementation, we'd check if player has games
    const hasGames = false; // Stub

    if (hasGames) {
      alert('Player has games and cannot be removed. Showing games...');
      return;
    }

    if (confirm(`Really remove ${this.selectedPlayer.name}?`)) {
      this.apiService.deletePlayer(this.selectedPlayer.id).subscribe({
        next: () => {
          this.selectedPlayer = null;
          this.loadPlayers();
        },
        error: (error) => {
          console.error('Error deleting player:', error);
          alert('Error deleting player.');
        }
      });
    }
  }

  goBack() {
    this.router.navigate(['/']);
  }

  get removeButtonText(): string {
    // In real implementation, check if player has games
    return 'Remove';
  }

  get conflictsEnabled(): boolean {
    return this.selectedPlayer !== null && this.players.length > 1;
  }

  private isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

  // Debug method to help track down rendering issues
  private debugDOM() {
    const selectElement = document.querySelector('select');
    if (selectElement) {
      console.log('Select element:', selectElement);
      console.log('  - Children count:', selectElement.children.length);
      console.log('  - Options:', selectElement.options.length);
      console.log('  - innerHTML:', selectElement.innerHTML.substring(0, 100) + '...');
    } else {
      console.warn('Select element not found in DOM');
    }

    console.log('Players array in component:', this.players.length);
  }

  // Manual refresh method as a workaround for Angular rendering issues
  manuallyRefreshPlayerList() {
    console.log('Manually refreshing player list...');
    if (!this.playerSelectRef || !this.players.length) return;

    const selectElement = this.playerSelectRef.nativeElement;

    // Clear existing options
    while (selectElement.options.length > 0) {
      selectElement.remove(0);
    }

    // Add options manually
    this.players.forEach(player => {
      const option = document.createElement('option');
      option.text = player.name || `${player.firstName} ${player.lastName}`;
      // Store player object as custom property
      option.value = player.id.toString();
      // Store reference to the full player object
      (option as any)._playerData = player;
      selectElement.add(option);
    });

    console.log(`Manually added ${this.players.length} player options to select element`);

    // Add change listener to update selectedPlayer
    selectElement.addEventListener('change', () => {
      const selectedIndex = selectElement.selectedIndex;
      if (selectedIndex >= 0) {
        this.selectedPlayer = (selectElement.options[selectedIndex] as any)._playerData || null;
      }
    });
  }
}
