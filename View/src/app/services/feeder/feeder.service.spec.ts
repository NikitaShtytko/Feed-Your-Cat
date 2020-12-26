import { TestBed } from '@angular/core/testing';

import { FeederService } from './feeder.service';

describe('FeederService', () => {
  let service: FeederService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FeederService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
