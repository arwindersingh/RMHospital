import { Component, OnInit } from '@angular/core';
import { PodAllocation } from '../../../dataservice/models/podallocation';
import { PodService } from '../../../dataservice/pod.service';
import { PodComponent } from '../pod.component'

@Component({
  selector: 'app-podd',
  templateUrl: './podd.component.html',
  styleUrls: ['./podd.component.css']
})

export class PodDComponent extends PodComponent implements OnInit {
  readonly teamName : string = 'd';
  readonly numBeds : number = 10;
}