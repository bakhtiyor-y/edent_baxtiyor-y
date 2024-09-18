import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceptInventorySettingEditFormComponent } from './recept-inventory-setting-edit-form.component';

describe('ReceptInventorySettingEditFormComponent', () => {
  let component: ReceptInventorySettingEditFormComponent;
  let fixture: ComponentFixture<ReceptInventorySettingEditFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReceptInventorySettingEditFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceptInventorySettingEditFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
