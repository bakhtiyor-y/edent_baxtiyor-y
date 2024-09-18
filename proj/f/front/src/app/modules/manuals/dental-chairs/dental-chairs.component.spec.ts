import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DentalChairsComponent } from './dental-chairs.component';

describe('DentalChairsComponent', () => {
  let component: DentalChairsComponent;
  let fixture: ComponentFixture<DentalChairsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DentalChairsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DentalChairsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
