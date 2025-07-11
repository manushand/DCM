import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, catchError, tap, throwError, of, retry, map } from 'rxjs';
import { Player, PlayerConflict } from '../models/player';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  // Use a proxy endpoint that's known to work with CORS
  private apiProxyUrl = '/api/proxy';
  private baseUrl = 'http://localhost:5104';

  constructor(private http: HttpClient) { }

  // Player endpoints
  getPlayers(): Observable<Player[]> {
    console.log('Fetching players...');

    // Try direct approach first
    return this.http.get<Player[]>('/players?details=true')
      .pipe(
        tap(() => console.log('HTTP request completed')),
        retry(2), // Retry up to 2 times
        map(response => {
          console.log('API raw response:', response);
          // Ensure we have an array to work with
          if (!response) {
            console.warn('Response is null or undefined, returning empty array');
            return [];
          }

          // Handle array response
          if (Array.isArray(response)) {
            console.log(`Response is an array with ${response.length} players`);
            // Create a new array with proper Player objects
            return response.map(p => {
              const player = {...p};
              if (!player.name && player.firstName && player.lastName) {
                player.name = `${player.firstName} ${player.lastName}`;
              }
              // Add timestamp to force change detection
              player._lastUpdated = Date.now();
              return player;
            });
          }

          // Handle object response with array property
          if (typeof response === 'object') {
            const possibleArrayProps = ['data', 'results', 'items', 'players'];
            for (const prop of possibleArrayProps) {
              if (Array.isArray((response as any)[prop])) {
                console.log(`Found array in response.${prop}`);
                return (response as any)[prop].map((p: any) => {
                  const player = {...p};
                  if (!player.name && player.firstName && player.lastName) {
                    player.name = `${player.firstName} ${player.lastName}`;
                  }
                  return player;
                });
              }
            }
          }

          console.error('Could not extract player array from response');
          return [];
        }),
        tap(players => console.log('Final processed players:', players)),
        catchError(err => {
          console.error('Error fetching players:', err);

          // If the API is down, provide mock data for testing
          if (err.status === 0) {
            console.warn('Using fallback mock data since API appears to be unavailable');
            return of([{
              id: 1,
              firstName: 'John',
              lastName: 'Doe',
              name: 'John Doe'
            }, {
              id: 2,
              firstName: 'Jane',
              lastName: 'Smith',
              name: 'Jane Smith'
            }]);
          }

          return throwError(() => err);
        })
      );
  }

  getPlayer(id: number): Observable<Player> {
    return this.http.get<Player>(`${this.baseUrl}/player/${id}`);
  }

  createPlayer(player: Player): Observable<any> {
    return this.http.post(`${this.baseUrl}/player`, player);
  }

  updatePlayer(player: Player): Observable<any> {
    return this.http.put(`${this.baseUrl}/player/${player.id}`, player);
  }

  deletePlayer(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/player/${id}`);
  }

  getPlayerConflicts(id: number): Observable<PlayerConflict[]> {
    return this.http.get<PlayerConflict[]>(`${this.baseUrl}/player/${id}/conflicts`);
  }

  setPlayerConflict(id: number, playerId: number, value?: number): Observable<PlayerConflict> {
    const url = `${this.baseUrl}/player/${id}/conflict/${playerId}`;
    // Always send a value when it's provided
    return this.http.patch<PlayerConflict>(url, value !== undefined ? { value } : {});
  }
}
