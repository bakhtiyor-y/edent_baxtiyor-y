import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewReceptComponent } from './view-recept.component';

describe('ViewReceptComponent', () => {
  let component: ViewReceptComponent;
  let fixture: ComponentFixture<ViewReceptComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ViewReceptComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewReceptComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
