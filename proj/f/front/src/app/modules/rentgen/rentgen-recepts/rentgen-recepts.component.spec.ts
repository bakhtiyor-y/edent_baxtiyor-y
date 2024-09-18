import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RentgenReceptsComponent } from './rentgen-recepts.component';

describe('RentgenReceptsComponent', () => {
  let component: RentgenReceptsComponent;
  let fixture: ComponentFixture<RentgenReceptsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RentgenReceptsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RentgenReceptsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
