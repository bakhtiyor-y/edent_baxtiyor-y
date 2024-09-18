import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RentgenReceptComponent } from './rentgen-recept.component';

describe('RentgenReceptComponent', () => {
  let component: RentgenReceptComponent;
  let fixture: ComponentFixture<RentgenReceptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RentgenReceptComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RentgenReceptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
