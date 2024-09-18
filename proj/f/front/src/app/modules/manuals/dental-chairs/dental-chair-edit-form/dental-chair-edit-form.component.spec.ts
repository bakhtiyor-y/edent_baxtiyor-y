import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DentalChairEditFormComponent } from './dental-chair-edit-form.component';

describe('DentalChairEditFormComponent', () => {
  let component: DentalChairEditFormComponent;
  let fixture: ComponentFixture<DentalChairEditFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DentalChairEditFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DentalChairEditFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
