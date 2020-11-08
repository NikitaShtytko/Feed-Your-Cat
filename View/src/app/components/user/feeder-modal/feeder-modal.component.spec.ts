import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FeederModalComponent } from './feeder-modal.component';

describe('FeederModalComponent', () => {
  let component: FeederModalComponent;
  let fixture: ComponentFixture<FeederModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FeederModalComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(FeederModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
