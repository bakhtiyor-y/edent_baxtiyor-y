import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CityEditFormComponent } from './city-edit-form.component';

describe('CityEditFormComponent', () => {
  let component: CityEditFormComponent;
  let fixture: ComponentFixture<CityEditFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CityEditFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CityEditFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
