import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceptReportComponent } from './recept-report.component';

describe('IncomeReportComponent', () => {
  let component: ReceptReportComponent;
  let fixture: ComponentFixture<ReceptReportComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReceptReportComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceptReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
