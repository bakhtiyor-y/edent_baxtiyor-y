import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SetTechnicComponent } from './set-technic.component';

describe('SetTechnicComponent', () => {
  let component: SetTechnicComponent;
  let fixture: ComponentFixture<SetTechnicComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SetTechnicComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SetTechnicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
