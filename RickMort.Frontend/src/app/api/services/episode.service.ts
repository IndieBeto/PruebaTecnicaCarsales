import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { EpisodeResult } from '../models/episode-result';

@Injectable({
  providedIn: 'root'
})
export class EpisodeService {
  private apiUrl = 'https://localhost:7138/api/Episodes';

  constructor(private http: HttpClient) { }

  getEpisodes(page: number = 1): Observable<EpisodeResult> {
    return this.http.get<EpisodeResult>(`${this.apiUrl}?page=${page}`)
      .pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    if (error.error instanceof ErrorEvent) {
      // A client-side or network error occurred. Handle it accordingly.
      console.error('An error occurred:', error.error.message);
    } else {
      // The backend returned an unsuccessful response code.
      console.error(`Backend returned code ${error.status}, ` + `body was: ${error.error}`);
    }
    // Return an observable with a user-facing error message.
    return throwError(
      'Something bad happened; please try again later.');
  }
}