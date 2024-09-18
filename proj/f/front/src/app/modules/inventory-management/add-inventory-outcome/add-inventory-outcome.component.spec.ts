import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddInventoryOutcomeComponent } from './add-inventory-outcome.component';

describe('AddInventoryOutcomeComponent', () => {
  let component: AddInventoryOutcomeComponent;
  let fixture: ComponentFixture<AddInventoryOutcomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddInventoryOutcomeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AddInventoryOutcomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
