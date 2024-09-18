import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DoctorEditAppointComponent } from './doctor-edit-appoint.component';

describe('DoctorEditAppointComponent', () => {
  let component: DoctorEditAppointComponent;
  let fixture: ComponentFixture<DoctorEditAppointComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DoctorEditAppointComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DoctorEditAppointComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
