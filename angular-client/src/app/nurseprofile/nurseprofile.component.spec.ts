import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NurseprofileComponent } from './nurseprofile.component';

describe('NurseprofileComponent', () => {
  let component: NurseprofileComponent;
  let fixture: ComponentFixture<NurseprofileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NurseprofileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NurseprofileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should be created', () => {
    expect(component).toBeTruthy();
  });
});
