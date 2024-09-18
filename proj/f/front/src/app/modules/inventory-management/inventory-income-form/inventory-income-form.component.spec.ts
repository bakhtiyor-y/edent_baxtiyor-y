import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryIncomeFormComponent } from './inventory-income-form.component';

describe('InventoryIncomeFormComponent', () => {
  let component: InventoryIncomeFormComponent;
  let fixture: ComponentFixture<InventoryIncomeFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InventoryIncomeFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InventoryIncomeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
