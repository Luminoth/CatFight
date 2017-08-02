import { TestBed, inject } from '@angular/core/testing';

import { GamedataService } from './gamedata.service';

describe('GamedataService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [GamedataService]
    });
  });

  it('should be created', inject([GamedataService], (service: GamedataService) => {
    expect(service).toBeTruthy();
  }));
});
