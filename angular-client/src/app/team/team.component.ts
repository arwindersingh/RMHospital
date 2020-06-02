import { Component, OnInit } from '@angular/core';
import { Allocation } from '../dataservice/models/allocation';
import { Staff } from '../dataservice/models/staff';
import { AllocationService } from '../dataservice/allocation.service';
import { Location } from '@angular/common';

/*
Team component which holds logic to deal with teams of staff (think the 
members of a pod). See pod.component for pod implementation of teams
and seniorstaff.component for the senior staff implementation.
This component is not used as it is currently.
*/

@Component({
  selector: 'app-team',
  template: "",
  providers: [AllocationService]
})

export class TeamComponent implements OnInit {
  alloc : Allocation;
  oldalloc : Allocation;
  public edited = true;
  public saveSuccess = false;
  readonly teamName : string;

  
  constructor(protected allocationService: AllocationService) {}
  
  ngOnInit() {}

  // init the shift value and color of the input area
  initColor(IDPrefix: string, numIds: number, isPod: boolean){
    // init the shift value and color of the nurse 
    for(let i =1;i<=numIds;i++){
      let buttn = document.getElementById(IDPrefix+i);
      if (!buttn) continue;
      let shiftType;

      if (isPod) {
        shiftType = this.getShiftType(i, isPod);
      }else{
        shiftType = this.getShiftType(buttn, isPod);
      }

      let button = buttn as HTMLButtonElement;

      switch (shiftType) {
        case '8':
          button.style.backgroundColor = 'lightblue';
          break;

        case '12':
          button.style.backgroundColor = 'mistyrose';
          break;

        case 'closed':
          button.style.backgroundColor = 'LightGray';
          break;

        default:
          button.style.backgroundColor = 'lightblue';
          break;
      }
    }
  }

  // Save the currently stored allocation to backend
  saveAlloc(){
    // Check for changes to nurse profiles, save them as strings
    for(let i in this.alloc){
      if(i!="beds"){
        if(this.alloc[i]["id"])
          if(this.alloc[i].staff_name!=this.oldalloc[i].staff_name)
            // If staff_name has changed, delete ID so it's saved as a string to the backend
            delete this.alloc[i].id
      }
      else{
        for(let j in this.alloc[i]){
          console.log("123:"+j);
          if(this.alloc[i][j]["id"])
            if(this.alloc[i][j].staff_name!=this.oldalloc[i][j].staff_name)
              // If staff_name has changed, delete ID so it's saved as a string to the backend
              delete this.alloc[i][j].id
        }
      }
    }
    this.allocationService.SaveAllocation(this.teamName,this.alloc);
    this.saveSuccess = true;
  }

  //Dismiss the save successful alert message
  dismissAlert(){
    this.saveSuccess = false;
  }

  // Swap the value of the "edited" boolean. 
  // This is used by Pods to enable/disable input into text boxes and display
  // extra editing components (radio buttons)
  editAlloc(){
    this.edited = !this.edited;
  }



  // Get the shift type (8 or 12 hours) of input element. 
  // ident : either the bed number of the pod, or the button element
  private getShiftType(ident, isPod : boolean){
    if (isPod) return this.alloc['beds'][ident-1].shift_type;
    let name = ident.getAttribute("ng-reflect-name");
    if (!name) name = ident.getAttribute("name");
    if (!name) return null;
    return this.alloc[name].shift_type
  }
}

