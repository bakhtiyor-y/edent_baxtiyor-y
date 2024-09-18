import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceptInventorySettingComponent } from './recept-inventory-setting.component';

describe('ReceptInventorySettingComponent', () => {
  let component: ReceptInventorySettingComponent;
  let fixture: ComponentFixture<ReceptInventorySettingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReceptInventorySettingComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceptInventorySettingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
