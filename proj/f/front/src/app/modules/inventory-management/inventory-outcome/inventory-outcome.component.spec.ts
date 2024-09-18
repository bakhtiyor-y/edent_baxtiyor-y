import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryOutcomeComponent } from './inventory-outcome.component';

describe('InventoryOutcomeComponent', () => {
  let component: InventoryOutcomeComponent;
  let fixture: ComponentFixture<InventoryOutcomeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InventoryOutcomeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InventoryOutcomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
