import { TestBed } from '@angular/core/testing';
import { DatabasesService } from './services/databases.service';
import { RestService } from '@abp/ng.core';

describe('DatabasesService', () => {
  let service: DatabasesService;
  const mockRestService = jasmine.createSpyObj('RestService', ['request']);
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: RestService,
          useValue: mockRestService,
        },
      ],
    });
    service = TestBed.inject(DatabasesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
