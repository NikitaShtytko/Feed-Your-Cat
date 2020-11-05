import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeedersPageComponent } from './feeders-page.component';

describe('FeedersPageComponent', () => {
  let component: FeedersPageComponent;
  let fixture: ComponentFixture<FeedersPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FeedersPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FeedersPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
