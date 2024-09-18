import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InvetntoryComponent } from './invetntory.component';

describe('InvetntoryComponent', () => {
  let component: InvetntoryComponent;
  let fixture: ComponentFixture<InvetntoryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ InvetntoryComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(InvetntoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
