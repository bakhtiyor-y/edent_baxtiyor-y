import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryIncomeComponent } from './inventory-income.component';

describe('InventoryIncomeComponent', () => {
  let component: InventoryIncomeComponent;
  let fixture: ComponentFixture<InventoryIncomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InventoryIncomeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InventoryIncomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
