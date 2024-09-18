import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CountryEditFormComponent } from './country-edit-form.component';

describe('CountryEditFormComponent', () => {
  let component: CountryEditFormComponent;
  let fixture: ComponentFixture<CountryEditFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CountryEditFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CountryEditFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
