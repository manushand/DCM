import { Injectable } from '@angular/core';
import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';

@Injectable()
export class CorsInterceptor implements HttpInterceptor {
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    // Create a new request with CORS headers
    const clonedRequest = request.clone({
      withCredentials: false,
      setHeaders: {
        'Content-Type': 'application/json',
        'Accept': 'application/json',
        // Don't set CORS headers from client side
        // The server must set these headers
      }
    });

    console.log(`Sending request to: ${request.url}`);

    return next.handle(clonedRequest).pipe(
      tap({
        next: (event: any) => {
          // Check for response events and log them for debugging
          if (event.body) {
            console.log(`Response from ${request.url} received:`);

            // Add specific debugging for player lists
            if (request.url.includes('players')) {
              const body = event.body;
              console.log('Player response body:', body);
              console.log('Is array?', Array.isArray(body));
              console.log('Length:', Array.isArray(body) ? body.length : 'N/A');

              if (Array.isArray(body) && body.length > 0) {
                console.log('First player:', body[0]);
              }
            }
          }
        },
        error: (error) => {
          console.error(`Request to ${request.url} failed:`, error);
          if (error.status === 0) {
            console.error('This is likely a CORS or network connectivity issue');
          }
        }
      })
    );
  }
}
