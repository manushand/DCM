import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-main-form',
  standalone: true,
  imports: [CommonModule, RouterOutlet],
  templateUrl: './main-form.html',
  styleUrl: './main-form.css'
})
export class MainFormComponent {
  activeMenu: string | null = null;
  tournamentName = 'DCM Version 2025\nStab You Soon!';
  showButtons = false;
  leftButtonText = 'Event Settings';
  middleButtonText = 'Registration';
  rightButtonText = 'Scores';
  middleButtonEnabled = false;
  rightButtonEnabled = false;

  constructor(private router: Router) {}

  setActiveMenu(menu: string) {
    this.activeMenu = this.activeMenu === menu ? null : menu;
  }

  navigateToPlayerManagement() {
    this.activeMenu = null;
    this.router.navigate(['/players']).finally();
  }

  navigateToPlayerConflicts() {
    this.activeMenu = null;
    this.router.navigate(['/player-conflicts']).finally();
  }

  showStub(functionality: string) {
    this.activeMenu = null;
    alert(`${functionality} functionality is stubbed out for now.`);
  }
}
