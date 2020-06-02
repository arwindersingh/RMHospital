import { Component, OnInit } from '@angular/core';
import { Headers, Http } from '@angular/http';  
import { PodAllocation } from '../dataservice/models/podallocation';
import { PodService } from '../dataservice/pod.service';
import { SeniorStaffAllocation } from '../dataservice/models/seniorstaffallocation';
import { SeniorStaffService } from '../dataservice/seniorstaff.service';
import { NoteService } from '../dataservice/nursenotes.service';
import { Staff } from '../dataservice/models/staff';
import { ChangeDetectorRef, ChangeDetectionStrategy } from "@angular/core";
import { Observable } from 'rxjs/Observable';
import { Subscription } from 'rxjs/Subscription';
import 'rxjs/Rx';
@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardComponent implements OnInit {
	poda: PodAllocation;
  podb: PodAllocation;
  podc: PodAllocation;
  podd: PodAllocation;
  senior: SeniorStaffAllocation;
  isConnected: Observable<boolean>;

  private timer;//timer used to check data update regularly.


   constructor(private podService: PodService, 
      private seniorStaffService: SeniorStaffService,
      private noteService: NoteService,  
      private ref: ChangeDetectorRef) {  
      
      this.timer = setInterval(()=> {
      this.ngOnInit();
      ref.markForCheck();
      }, 800)

      this.isConnected = Observable.merge(
      Observable.of(navigator.onLine),
      Observable.fromEvent(window, 'online').map(() => true),
      Observable.fromEvent(window, 'offline').map(() => false));
  }

  ngOnInit() {
  	  this.podService.getHttpAllocation('a').then(pod => {
      this.poda = pod;
      this.getNotes(this.poda);
     });

      this.podService.getHttpAllocation('b').then(pod => {
      this.podb = pod;
      this.getNotes(this.podb);
     });

      this.podService.getHttpAllocation('c').then(pod => {
      this.podc = pod;
      this.getNotes(this.podc);
     });

      this.podService.getHttpAllocation('d').then(pod => {
      this.podd = pod;
      this.getNotes(this.podd);
     });
      this.seniorStaffService.getHttpAllocation('senior').then(seniorstaff => {
      this.senior = seniorstaff;
     });
  }

  // Populate staff within allocation with their dashboard notes
  private getNotes(alloc:PodAllocation) {
    for(let position in alloc) {
      if(alloc[position]['id']){
        this.noteService.getHttpNotes(alloc[position]['id']).then( notes => {
          alloc[position]['notes'] = notes
        });
      } 
    }
    for(let nurse of alloc['beds']) {
      if(nurse.id){
        this.noteService.getHttpNotes(nurse.id).then( notes => {
          nurse.notes = notes
        });
      }
    }
  }

  // Looks into the senior allocation and extracts staff members under "name"
  // Returns array of non-empty names
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

   ngOnDestroy() {  //...
    if (this.timer) {  
      clearInterval(this.timer);  
    }  
  }
}
