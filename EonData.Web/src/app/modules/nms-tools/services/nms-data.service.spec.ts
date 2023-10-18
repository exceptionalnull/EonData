import { TestBed } from '@angular/core/testing';

import { NmsDataService } from './nms-data.service';

describe('NmsDataService', () => {
  let service: NmsDataService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(NmsDataService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
