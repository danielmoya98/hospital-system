import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TrackingCenter } from './tracking-center';

describe('TrackingCenter', () => {
  let component: TrackingCenter;
  let fixture: ComponentFixture<TrackingCenter>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TrackingCenter],
    }).compileComponents();

    fixture = TestBed.createComponent(TrackingCenter);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
