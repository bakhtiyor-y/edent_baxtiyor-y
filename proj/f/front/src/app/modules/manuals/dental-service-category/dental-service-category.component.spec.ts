import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DentalServiceCategoryComponent } from './dental-service-category.component';

describe('DentalServiceCategoryComponent', () => {
  let component: DentalServiceCategoryComponent;
  let fixture: ComponentFixture<DentalServiceCategoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DentalServiceCategoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DentalServiceCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
