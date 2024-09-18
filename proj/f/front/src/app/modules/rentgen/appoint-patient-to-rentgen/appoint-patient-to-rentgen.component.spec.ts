import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointPatientToRentgenComponent } from './appoint-patient-to-rentgen.component';

describe('AppointPatientToRentgenComponent', () => {
  let component: AppointPatientToRentgenComponent;
  let fixture: ComponentFixture<AppointPatientToRentgenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppointPatientToRentgenComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppointPatientToRentgenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
