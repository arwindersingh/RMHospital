import { Injectable } from '@angular/core';
import { AllocationService } from './allocation.service'
import { Staff } from './models/staff';
import { SeniorStaffAllocation } from './models/seniorstaffallocation';
import { Headers, Http } from '@angular/http';  
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';

@Injectable()
export class SeniorStaffService extends AllocationService {

  allocation : SeniorStaffAllocation;
  protected url = 'api/team/senior';  // URL to web api

  constructor(protected http: Http) {
    super(http);
  }

  // get the current senior staff allocation
  getHttpAllocation(teamName: string): Promise<SeniorStaffAllocation> {
    return this.http.get(this.url)
      .toPromise()
      .then(response => response.json().team as SeniorStaffAllocation)
      .then(alloc => this.dataValidation(alloc))
      .catch(this.handleError);
  }

  // get senior staff allocation history according to the time stamp
  getHttpAllocationTime(teamName: string, timestamp: number): Promise<SeniorStaffAllocation> {
    const urlTime = `${this.url}?t=${timestamp}`;
    return this.http.get(urlTime)
      .toPromise()
      .then(response => response.json().team as SeniorStaffAllocation)
      .then(alloc => this.dataValidation(alloc))
      .catch(this.handleError);
  }

  putHttpAllocation(name: string, senior: SeniorStaffAllocation): Promise<SeniorStaffAllocation>{
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.put(this.url, JSON.stringify({team:senior}, this.jsonReplacer), {headers: headers}) 
      .toPromise() 
      .then(()=>senior)
      .catch(this.handleError);
  }

  // convert staff members to only include staff_name and staff_type in json object
  private jsonReplacer(key,value){
    if (['last_double', 'skills', 'photo', 
        'staff_id', 'staff_type'].some(k => k == key)){
      //stringify removes all undefined values from JSON string
      return undefined;
    }
    return value;
  }

  // Run through a SeniorStaffAllocation and ensure all fields are 
  // initalised and not null
  private dataValidation(allocation: SeniorStaffAllocation){
    if (!allocation){
      allocation = new SeniorStaffAllocation('senior');
    }
    let multiStaff : any[] = [
      ['cnm', 2], ['cnc',2],
      ['educator', 5], ['resource',4],
      ['internal', 2], ['external',2]
    ];
    for (let p of multiStaff){
      if (allocation[p[0]]){
        allocation[p[0]] = this.minArrLen(allocation[p[0]], p[1]);
      } else {
        allocation[p[0]] = this.minArrLen([], p[1]);
      }
    }
    let snglStaff : string[] = [
      'access_coordinator', 'tech',
      'mern', 'ca_support',
      'ward_consultant', 'transport',
      'donation'
    ];
    for (let p of snglStaff){
      if (!allocation[p]){
        allocation[p] = new Staff("","8");
      }
    }
    return allocation;
  }

  // Pads out arrays shorter than i with blank staff members
  private minArrLen(arr: Staff[], i: number){
    while(arr.length < i){
          arr.push(new Staff("","8"));
        }
    return arr;
  }

}