import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorAppointPatientComponent } from './doctor-appoint-patient.component';

describe('DoctorAppointPatientComponent', () => {
  let component: DoctorAppointPatientComponent;
  let fixture: ComponentFixture<DoctorAppointPatientComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DoctorAppointPatientComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DoctorAppointPatientComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
