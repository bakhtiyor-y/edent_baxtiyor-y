import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RentgenDeskComponent } from './rentgen-desk.component';

describe('RentgenDeskComponent', () => {
  let component: RentgenDeskComponent;
  let fixture: ComponentFixture<RentgenDeskComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RentgenDeskComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RentgenDeskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
