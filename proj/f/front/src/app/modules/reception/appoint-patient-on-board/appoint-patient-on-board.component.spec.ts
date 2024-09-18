import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AppointPatientOnBoardComponent } from './appoint-patient-on-board.component';

describe('AppointPatientOnBoardComponent', () => {
  let component: AppointPatientOnBoardComponent;
  let fixture: ComponentFixture<AppointPatientOnBoardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AppointPatientOnBoardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppointPatientOnBoardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
