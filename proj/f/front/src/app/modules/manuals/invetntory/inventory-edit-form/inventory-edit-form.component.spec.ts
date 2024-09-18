import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InventoryEditFormComponent } from './inventory-edit-form.component';

describe('InventoryEditFormComponent', () => {
  let component: InventoryEditFormComponent;
  let fixture: ComponentFixture<InventoryEditFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InventoryEditFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InventoryEditFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
