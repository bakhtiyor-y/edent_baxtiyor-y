import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DentalServiceComponent } from './dental-service.component';

describe('DentalServiceComponent', () => {
  let component: DentalServiceComponent;
  let fixture: ComponentFixture<DentalServiceComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DentalServiceComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(DentalServiceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
