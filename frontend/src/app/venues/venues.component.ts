import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { catchError, of, startWith, map } from 'rxjs';
import { VenueService } from './venue.service';
import { Venue } from './venue.model';

interface ViewState {
  loading: boolean;
  error: boolean;
  venues: Venue[];
}

@Component({
  selector: 'app-venues',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [CommonModule, MatTableModule, MatProgressSpinnerModule],
  template: `
    @if (state().loading) {
      <mat-spinner diameter="48" />
    } @else if (state().error) {
      <p class="error">Unable to load venues. Please try again later.</p>
    } @else if (state().venues.length === 0) {
      <p>No venues available.</p>
    } @else {
      <table mat-table [dataSource]="state().venues" class="mat-elevation-z2">
        <ng-container matColumnDef="name">
          <th mat-header-cell *matHeaderCellDef>Name</th>
          <td mat-cell *matCellDef="let v">{{ v.name }}</td>
        </ng-container>
        <ng-container matColumnDef="capacity">
          <th mat-header-cell *matHeaderCellDef>Capacity</th>
          <td mat-cell *matCellDef="let v">{{ v.capacity }}</td>
        </ng-container>
        <ng-container matColumnDef="city">
          <th mat-header-cell *matHeaderCellDef>City</th>
          <td mat-cell *matCellDef="let v">{{ v.city }}</td>
        </ng-container>
        <tr mat-header-row *matHeaderRowDef="columns"></tr>
        <tr mat-row *matRowDef="let row; columns: columns"></tr>
      </table>
    }
  `,
  styles: [`
    .error { color: #d32f2f; padding: 16px; }
    table { width: 100%; }
  `]
})
export class VenuesComponent {
  protected readonly columns = ['name', 'capacity', 'city'];

  private readonly venueService = inject(VenueService);

  protected readonly state = toSignal(
    this.venueService.list().pipe(
      map((venues): ViewState => ({ loading: false, error: false, venues })),
      catchError(() => of<ViewState>({ loading: false, error: true, venues: [] })),
      startWith<ViewState>({ loading: true, error: false, venues: [] })
    ),
    { initialValue: { loading: true, error: false, venues: [] } as ViewState }
  );
}
