import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DentalServiceReportComponent } from './dental-service-report.component';

describe('DentalServiceReportComponent', () => {
  let component: DentalServiceReportComponent;
  let fixture: ComponentFixture<DentalServiceReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DentalServiceReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DentalServiceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
