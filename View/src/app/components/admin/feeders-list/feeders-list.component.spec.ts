import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedersListComponent } from './feeders-list.component';

describe('FeedersListComponent', () => {
  let component: FeedersListComponent;
  let fixture: ComponentFixture<FeedersListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FeedersListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FeedersListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
