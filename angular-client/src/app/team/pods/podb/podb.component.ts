import { Component, OnInit } from '@angular/core';
import { PodAllocation } from '../../../dataservice/models/podallocation';
import { PodService } from '../../../dataservice/pod.service';
import { PodComponent } from '../pod.component'

@Component({
  selector: 'app-podb',
  templateUrl: './podb.component.html',
  styleUrls: ['./podb.component.css']
})

export class PodBComponent extends PodComponent implements OnInit {
  readonly teamName : string = 'b';
  readonly numBeds : number = 10;
}