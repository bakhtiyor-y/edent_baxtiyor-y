import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PatientToothHistoryComponent } from './patient-tooth-history.component';

describe('PatientToothHistoryComponent', () => {
  let component: PatientToothHistoryComponent;
  let fixture: ComponentFixture<PatientToothHistoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PatientToothHistoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PatientToothHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
