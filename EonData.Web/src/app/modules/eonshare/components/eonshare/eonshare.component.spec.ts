import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EonshareComponent } from './eonshare.component';

describe('EonshareComponent', () => {
  let component: EonshareComponent;
  let fixture: ComponentFixture<EonshareComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EonshareComponent]
    });
    fixture = TestBed.createComponent(EonshareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
