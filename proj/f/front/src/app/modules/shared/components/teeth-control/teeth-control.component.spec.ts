import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeethControlComponent } from './teeth-control.component';

describe('TeethControlComponent', () => {
  let component: TeethControlComponent;
  let fixture: ComponentFixture<TeethControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TeethControlComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TeethControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
