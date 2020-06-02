import { Component, OnInit } from '@angular/core';
import { PodAllocation } from '../../../dataservice/models/podallocation';
import { PodService } from '../../../dataservice/pod.service';
import { PodComponent } from '../pod.component';

@Component({
  selector: 'app-poda',
  templateUrl: './poda.component.html',
  styleUrls: ['./poda.component.css'],
  providers: [PodService]
})

export class PodAComponent extends PodComponent implements OnInit {
  readonly teamName : string = 'a';
  readonly numBeds : number = 12;
}