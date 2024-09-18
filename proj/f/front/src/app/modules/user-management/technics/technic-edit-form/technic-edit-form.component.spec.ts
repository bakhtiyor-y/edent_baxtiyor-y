import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TechnicEditFormComponent } from './technic-edit-form.component';

describe('TechnicEditFormComponent', () => {
  let component: TechnicEditFormComponent;
  let fixture: ComponentFixture<TechnicEditFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TechnicEditFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TechnicEditFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
