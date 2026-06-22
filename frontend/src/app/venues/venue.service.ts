import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Venue } from './venue.model';
import { environment } from '../../environments/environment';

@Injectable({ providedIn: 'root' })
export class VenueService {
  private readonly http = inject(HttpClient);

  list(): Observable<Venue[]> {
    return this.http.get<Venue[]>(`${environment.apiUrl}/api/venues`);
  }
}
