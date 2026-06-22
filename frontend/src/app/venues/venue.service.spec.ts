import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { VenueService } from './venue.service';
import { environment } from '../../environments/environment';

describe('VenueService', () => {
  let service: VenueService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        VenueService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(VenueService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => httpMock.verify());

  it('should GET venues from the correct endpoint', () => {
    const mockVenues = [
      { id: 1, name: 'Auditorio Central', capacity: 200, city: 'Bogotá' },
      { id: 2, name: 'Sala Norte', capacity: 50, city: 'Bogotá' },
      { id: 3, name: 'Arena Sur', capacity: 500, city: 'Medellín' }
    ];

    let result: typeof mockVenues | undefined;
    service.list().subscribe(v => (result = v));

    const req = httpMock.expectOne(`${environment.apiUrl}/api/venues`);
    expect(req.request.method).toBe('GET');
    req.flush(mockVenues);

    expect(result).toHaveLength(3);
    expect(result![0].name).toBe('Auditorio Central');
  });
});
