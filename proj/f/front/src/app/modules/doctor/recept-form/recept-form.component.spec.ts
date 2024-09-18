import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReceptFormComponent } from './recept-form.component';

describe('ReceptFormComponent', () => {
  let component: ReceptFormComponent;
  let fixture: ComponentFixture<ReceptFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ReceptFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ReceptFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
