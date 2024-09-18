import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegionEditFormComponent } from './region-edit-form.component';

describe('RegionEditFormComponent', () => {
  let component: RegionEditFormComponent;
  let fixture: ComponentFixture<RegionEditFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegionEditFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegionEditFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
