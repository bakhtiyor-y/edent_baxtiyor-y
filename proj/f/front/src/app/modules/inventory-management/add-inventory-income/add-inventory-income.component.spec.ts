import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddInventoryIncomeComponent } from './add-inventory-income.component';

describe('AddInventoryIncomeComponent', () => {
  let component: AddInventoryIncomeComponent;
  let fixture: ComponentFixture<AddInventoryIncomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddInventoryIncomeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddInventoryIncomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
