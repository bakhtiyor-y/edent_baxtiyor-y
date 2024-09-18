import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DentalServicePriceComponent } from './dental-service-price.component';

describe('DentalServicePriceComponent', () => {
  let component: DentalServicePriceComponent;
  let fixture: ComponentFixture<DentalServicePriceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DentalServicePriceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DentalServicePriceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
