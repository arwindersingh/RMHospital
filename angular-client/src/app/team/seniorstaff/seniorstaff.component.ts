import { Component, OnInit } from '@angular/core';

import { SeniorStaffAllocation } from '../../dataservice/models/seniorstaffallocation';
import { Staff } from '../../dataservice/models/staff';
import { SeniorStaffService } from '../../dataservice/seniorstaff.service';
import { TeamComponent } from '../team.component'

/*
Senior staff component to provide logic and functionality for 
senior staff teams.
*/

@Component({
  selector: 'app-seniorstaff',
  templateUrl: './seniorstaff.component.html',
  styleUrls: ['./seniorstaff.component.css'],
  providers: [SeniorStaffService]
})

export class SeniorStaffComponent extends TeamComponent implements OnInit {

	alloc : SeniorStaffAllocation;
  readonly teamName : string = 'senior';


  constructor(private seniorStaffService: SeniorStaffService) {super(seniorStaffService)}

  ngOnInit() {
   	this.seniorStaffService.getHttpAllocation(this.teamName).then(staff => {
      this.alloc = staff;
     });
  }
}

