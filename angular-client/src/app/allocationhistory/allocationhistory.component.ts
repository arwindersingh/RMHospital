import { Component, OnInit } from '@angular/core';
import { Headers, Http } from '@angular/http';  
import { IMyOptions, IMyDate, IMyDateModel} from 'mydatepicker';
import { PodAllocation } from '../dataservice/models/podallocation';
import { SeniorStaffAllocation } from '../dataservice/models/seniorstaffallocation';
import { SeniorStaffService } from '../dataservice/seniorstaff.service';
import { Staff } from '../dataservice//models/staff';
import { PodService } from '../dataservice/pod.service';

@Component({
  selector: 'app-allocationhistory',
  templateUrl: './allocationhistory.component.html',
  styleUrls: ['./allocationhistory.component.css']
})
export class AllocationHistoryComponent implements OnInit {
	poda: PodAllocation;
  podb: PodAllocation;
  podc: PodAllocation;
  podd: PodAllocation;
  senior: SeniorStaffAllocation;

  myDatePickerOptions: IMyOptions = {
        // other options...
        dateFormat: 'dd/mm/yyyy',
        showClearDateBtn: false,
    };
    
    // Initialized to specific date     
    selDate: IMyDate = {year: 0, month: 0, day: 0};
    
    timeList:any = [
        ];
    time:number;

  constructor(private seniorStaffService: SeniorStaffService, private podService: PodService,) { }

  ngOnInit() {
      //initialize the time
      this.time = 0;
      //initialize the select list from 0-23:00
      for (let i = 0; i < 24 ; i++){
        this.timeList.push({id:i, name:i +':00'});
      }
      //console.log(this.timeList);
      
      this.podService.getHttpAllocation('a').then(pod => {
      this.poda = pod;
     });

      this.podService.getHttpAllocation('b').then(pod => {
      this.podb = pod;
     });

      this.podService.getHttpAllocation('c').then(pod => {
      this.podc = pod;
     });

      this.podService.getHttpAllocation('d').then(pod => {
      this.podd = pod;
     });
      this.seniorStaffService.getHttpAllocation('senior').then(seniorstaff => {
      this.senior = seniorstaff;
     });
  }

  //display multiple staff
  expandStaffList(name): string[]{
    if(!this.senior) return [];
    if(!this.senior[name]) return [];
    var names : string[] = [];
    if (this.senior[name] instanceof Array){
      this.senior[name].forEach(s => {if(s.staff_name) { names.push(s.staff_name);}});
      return names;
    } else {
      return [this.senior[name].staff_name]
    }
  }

  // when date is changed, Update value of selDate variable
  onDateChanged(event: IMyDateModel) {
        // Update value of selDate variable
        this.selDate = event.date;
  }

 //search the allocation history according to the chosen time
  searchHistory(){

    if(this.selDate.day == 0 || this.selDate.month == 0 || this.selDate.year == 0)
      {
        alert("Date can not be empty!");
      }else{
      // var timeStr = this.selDate.year+"/"+this.selDate.month+"/"+this.selDate.day+" "+this.time+":00:00";  
      // var timestamp = Date.parse(timeStr);
      // timestamp = timestamp/1000;

      // get the timestamp(compatible with IE)
      //{year: 2017, month: 9, day: 7}

     
      var time1 = new Date();
      time1.setUTCFullYear(this.selDate.year);
      time1.setUTCMonth(this.selDate.month-1);
      time1.setUTCDate(this.selDate.day);
      time1.setUTCHours(this.time);
      time1.setUTCMinutes(0);
      time1.setUTCSeconds(0);
      time1.setUTCMilliseconds(0);
      var timestamp1 = Math.floor(time1.getTime() / 1000);

      // adjust the local time zone
      var time2 = new Date();
      var timestamp2 = time2.getTimezoneOffset()*60;
      var timestmp = timestamp1 + timestamp2;
      console.log(timestmp);
      //alert(timestmp);

      //get each pod allocation history
        this.podService.getHttpAllocationHistory('a',timestmp).then(pod => {
        this.poda = pod;
      });

        this.podService.getHttpAllocationHistory('b',timestmp).then(pod => {
        this.podb = pod;
      });

        this.podService.getHttpAllocationHistory('c',timestmp).then(pod => {
        this.podc = pod;
      });

        this.podService.getHttpAllocationHistory('d',timestmp).then(pod => {
        this.podd = pod;
      });

      //get senior staff history
        this.seniorStaffService.getHttpAllocationTime('senior',timestmp).then(seniorstaff => {
        this.senior = seniorstaff;
      });
    }

  }


}
