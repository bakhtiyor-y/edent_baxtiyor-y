import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProvideDiscountComponent } from './provide-discount.component';

describe('ProvideDiscountComponent', () => {
  let component: ProvideDiscountComponent;
  let fixture: ComponentFixture<ProvideDiscountComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProvideDiscountComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProvideDiscountComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
