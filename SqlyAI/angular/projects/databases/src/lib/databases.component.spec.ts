import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { DatabasesComponent } from './components/databases.component';
import { DatabasesService } from '@sqly-aI/databases';
import { of } from 'rxjs';

describe('DatabasesComponent', () => {
  let component: DatabasesComponent;
  let fixture: ComponentFixture<DatabasesComponent>;
  const mockDatabasesService = jasmine.createSpyObj('DatabasesService', {
    sample: of([]),
  });
  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [DatabasesComponent],
      providers: [
        {
          provide: DatabasesService,
          useValue: mockDatabasesService,
        },
      ],
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DatabasesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
