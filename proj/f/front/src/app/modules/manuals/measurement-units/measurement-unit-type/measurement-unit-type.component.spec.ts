import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MeasurementUnitTypeComponent } from './measurement-unit-type.component';

describe('MeasurementUnitTypeComponent', () => {
  let component: MeasurementUnitTypeComponent;
  let fixture: ComponentFixture<MeasurementUnitTypeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MeasurementUnitTypeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MeasurementUnitTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
