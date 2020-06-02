import { Component, OnInit } from '@angular/core';
import { PodAllocation } from '../../dataservice/models/podallocation';
import { Staff } from '../../dataservice/models/staff';
import { PodService } from '../../dataservice/pod.service';
import { TeamComponent } from '../team.component'
import { CellComponent } from '../shared/cell.component'

/*
Pod component to provide logic and functionality for pod teams.
*/

@Component({
  selector: 'app-pod',
  template: "",
  providers: [PodService]
})

export class PodComponent extends TeamComponent implements OnInit {
  alloc : PodAllocation;
  oldalloc : PodAllocation;
  readonly teamName : string;
  readonly numBeds : number;
  readonly numCentral : number = 6;

  constructor(protected podService: PodService) {super(podService);}
  
  ngOnInit() {
    this.alloc = this.podService.validateAllocation(null, this.teamName, this.numBeds);
     this.podService.getHttpAllocation(this.teamName)
     .then(pod => {
      this.alloc = this.podService.validateAllocation(pod, this.teamName, this.numBeds);
      this.initColor(this.teamName.toUpperCase(), this.numBeds, true);
      this.initColor('L', this.numCentral, false);
      this.oldalloc =  JSON.parse(JSON.stringify(this.alloc));
     })
  }
}