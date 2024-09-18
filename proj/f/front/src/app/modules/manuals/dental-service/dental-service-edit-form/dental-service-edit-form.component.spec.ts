import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DentalServiceEditFormComponent } from './dental-service-edit-form.component';

describe('DentalServiceEditFormComponent', () => {
  let component: DentalServiceEditFormComponent;
  let fixture: ComponentFixture<DentalServiceEditFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DentalServiceEditFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DentalServiceEditFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
