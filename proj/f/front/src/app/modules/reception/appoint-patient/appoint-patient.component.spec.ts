import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointPatientComponent } from './appoint-patient.component';

describe('AppointPatientComponent', () => {
  let component: AppointPatientComponent;
  let fixture: ComponentFixture<AppointPatientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppointPatientComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppointPatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
