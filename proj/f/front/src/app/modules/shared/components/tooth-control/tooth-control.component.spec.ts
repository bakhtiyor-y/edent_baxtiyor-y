import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ToothControlComponent } from './tooth-control.component';

describe('ToothControlComponent', () => {
  let component: ToothControlComponent;
  let fixture: ComponentFixture<ToothControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ToothControlComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ToothControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
