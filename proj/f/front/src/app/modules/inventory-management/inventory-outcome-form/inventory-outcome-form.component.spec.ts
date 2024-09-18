import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryOutcomeFormComponent } from './inventory-outcome-form.component';

describe('InventoryOutcomeFormComponent', () => {
  let component: InventoryOutcomeFormComponent;
  let fixture: ComponentFixture<InventoryOutcomeFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InventoryOutcomeFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InventoryOutcomeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
