import { Component, OnInit } from '@angular/core';
import { PodAllocation } from '../../../dataservice/models/podallocation';
import { PodService } from '../../../dataservice/pod.service';
import { PodComponent } from '../pod.component'

@Component({
  selector: 'app-podc',
  templateUrl: './podc.component.html',
  styleUrls: ['./podc.component.css']
})

export class PodCComponent extends PodComponent implements OnInit {
  readonly teamName : string = 'c';
  readonly numBeds : number = 10;
}
