import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DentalServiceGroupComponent } from './dental-service-group.component';

describe('DentalServiceGroupComponent', () => {
  let component: DentalServiceGroupComponent;
  let fixture: ComponentFixture<DentalServiceGroupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DentalServiceGroupComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DentalServiceGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
