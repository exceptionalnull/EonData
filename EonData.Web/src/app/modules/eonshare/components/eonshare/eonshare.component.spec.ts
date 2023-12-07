import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EonShareComponent } from './eonshare.component';

describe('EonshareComponent', () => {
  let component: EonShareComponent;
  let fixture: ComponentFixture<EonShareComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [EonShareComponent]
    });
    fixture = TestBed.createComponent(EonShareComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
