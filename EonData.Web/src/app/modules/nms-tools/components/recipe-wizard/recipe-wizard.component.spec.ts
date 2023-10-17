import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecipeWizardComponent } from './recipe-wizard.component';

describe('RecipeWizardComponent', () => {
  let component: RecipeWizardComponent;
  let fixture: ComponentFixture<RecipeWizardComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [RecipeWizardComponent]
    });
    fixture = TestBed.createComponent(RecipeWizardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
