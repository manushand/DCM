import { Component, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HttpClientModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class AppComponent implements OnInit {
  title = 'DCM - Diplomacy Competition Manager';

  ngOnInit() {
    // Ensure favicon is set after component initialization
    const link = document.querySelector("link[rel~='icon']") as HTMLLinkElement;
    if (!link) {
      const newLink = document.createElement('link');
      newLink.rel = 'icon';
      newLink.type = 'image/x-icon';
      newLink.href = 'favicon.ico';
      document.head.appendChild(newLink);
    } else if (!link.href) {
      link.href = 'favicon.ico';
    }
  }
}
