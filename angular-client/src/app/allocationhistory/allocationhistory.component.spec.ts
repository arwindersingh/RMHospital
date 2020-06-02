import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllocationHistoryComponent } from './allocationhistory.component';
//import { AllocationHistoryService } from '../dataservice/allocationhistory.service';
import { SeniorStaffService } from '../dataservice/seniorstaff.service';
import { PodService } from '../dataservice/pod.service';
import { HttpModule } from '@angular/http';
import {FormsModule} from '@angular/forms';
describe('AllocationHistoryComponent', () => {
  let component: AllocationHistoryComponent;
  let fixture: ComponentFixture<AllocationHistoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpModule,
        FormsModule
      ],
      declarations: [ AllocationHistoryComponent],
 //     providers: [PodService,AllocationHistoryService, SeniorStaffService]
 
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllocationHistoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
