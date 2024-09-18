import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientsReportComponent } from './patients-report.component';

describe('PatientsReportComponent', () => {
  let component: PatientsReportComponent;
  let fixture: ComponentFixture<PatientsReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PatientsReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientsReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
