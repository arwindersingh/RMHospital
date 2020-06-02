import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { SeniorStaffComponent } from './seniorstaff.component';
import { HttpModule } from '@angular/http';

describe('SeniorStaffComponent', () => {
  let component: SeniorStaffComponent;
  let fixture: ComponentFixture<SeniorStaffComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ FormsModule, HttpModule],
      declarations: [ SeniorStaffComponent]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeniorStaffComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
describe ('manually add nurses' , function(){
    beforeEach(function() {
      
    });
    it('should should add nurses');
});