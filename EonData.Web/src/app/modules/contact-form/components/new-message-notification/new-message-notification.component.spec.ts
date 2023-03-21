import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewMessageNotificationComponent } from './new-message-notification.component';

describe('NewMessageNotificationComponent', () => {
  let component: NewMessageNotificationComponent;
  let fixture: ComponentFixture<NewMessageNotificationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewMessageNotificationComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewMessageNotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
