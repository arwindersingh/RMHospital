import { Injectable} from '@angular/core';
import { AllocationService } from './allocation.service'
import { NurseProfileService } from './nurseprofile.service'
import { Staff } from './models/staff';
import { Note } from './models/note';
import { PodAllocation } from './models/podallocation';
import { Headers, Http } from '@angular/http';  
import 'rxjs/add/operator/toPromise';
import * as Rx from 'rxjs/Rx'

@Injectable()
export class PodService extends AllocationService {

  allocation : PodAllocation;
  staffList : Promise<Staff[]>;

  constructor(protected http: Http, private nurseProfileService:NurseProfileService) {
    super(http);   
    this.staffList = this.nurseProfileService.getHttpAllStaff();
  }


  getHttpAllocation(id: string): Promise<PodAllocation> {
    const url = `${this.url}/${id}`;
    return Rx.Observable.forkJoin(
        this.http.get(url).toPromise(), 
        this.staffList
      ).toPromise()
      .then(vals => 
        this.allocationReviver(vals[0].json().team, vals[1]))
      .catch(this.handleError);
  }

  putHttpAllocation(id: string, pod: PodAllocation): Promise<PodAllocation>{
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    const url = `${this.url}/${id}`;
    return this.http.put(url,JSON.stringify({team:pod}, this.jsonReplacer), {headers: headers}) 
      .toPromise() 
      .then(()=>pod)
      .catch(this.handleError);
  }

  putHttpSidebar(id: string, pod: Object): Promise<PodAllocation>{
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let url = "api/team/" + id;
    return this.http.put(url,JSON.stringify({team:pod}), {headers: headers}) 
      .toPromise() 
      .then(()=>pod)
      .catch(this.handleError);
  }

  getHttpNurse(id:string): Promise<any> {
    return this.http.get("/api/staff/"+id)
      .toPromise()
      .then(response => response.json())
      .catch(this.handleError);
  }

  //get pod allocation history with timestamp
  getHttpAllocationHistory(id: string, timestamp: number): Promise<PodAllocation> {
  const url = `${this.url}/${id}?t=${timestamp}`;
  return Rx.Observable.forkJoin(
        this.http.get(url).toPromise(), 
        this.staffList
      ).toPromise()
      .then(vals => 
        this.allocationReviver(vals[0].json().team, vals[1]) as PodAllocation
      )
      .catch(this.handleError);
  }
     
  // Run through a PodAllocation and ensure all fields are 
  // initalised and not null given its pod ID and number of beds
  validateAllocation(allocation: PodAllocation, id: string, numBeds: number){
    if (!allocation){
      allocation = new PodAllocation(id);
    }
    let snglStaff : string[] = [
      'consultant', 'registrar',
      'pod_ca', 'team_leader',
      'resident', 'ca_cleaner'
    ];
    for (let p of snglStaff){
      if (!allocation[p]){
        allocation[p] = new Staff("","8");
      }
    }
    if (!allocation["beds"]){
      allocation["beds"] = [];
    }
    let len = allocation["beds"].length;
    while (len < numBeds){
      allocation["beds"].push(new Staff("","8"));
      len++;
    }
    return allocation;
  }  

  // convert the 'beds' array in PodAllocation to fields
  // convert Staff objects to {staff_name: ..., shift_type: ..} objects
  private jsonReplacer(key,value){
    let staffToAPI = (staff:Staff) => {
        let apiStaff:object = {};
        apiStaff["shift_type"] = staff.shift_type;
       
        if (staff.id){
          apiStaff["staff_id"] = staff.id;
       
        } else {
          apiStaff["staff_name"] = staff.staff_name;
        }

        return apiStaff;    
    }
    if (key == 'beds'){
      let beds: object = {};
      let i: number = 1;
      for (let bednum in value){
        beds[(i).toString()] = staffToAPI(value[bednum]);
        i = i + 1;
      }
      return beds;
    }
    //Don't replace objects with "staff_id" as they've already been proccessed
    if (value && value.hasOwnProperty("shift_type") 
        && !value.hasOwnProperty("staff_id")){
      return staffToAPI(value);
    }

    return value;
  }
  private staffTypeReviver(pods: PodAllocation):PodAllocation{
    this.staffList.then(s=>{
      for(let p in pods["beds"]){
        if(pods["beds"][p].id!=null){
          if(pods["beds"][p].staff_name!= s[pods["beds"][p].id].staff_name){        
             pods["beds"][p] = new Staff(pods["beds"][p].staff_name);
          }
        }  
      } 
    });
    return pods;
  }
  private allocationReviver(json, staffList:Staff[]):PodAllocation{ 
      let allocation = {};
      let ids: string[] = [];
      let beds: Staff[] = []
      for(let bednum in json['beds']){
          //staff_id
          if(json['beds'][bednum]['staff_id'] != undefined){
             let staff:Staff =  this.findStaff(json['beds'][bednum]['staff_id'], staffList);
             staff.shift_type = json['beds'][bednum]['shift_type'].toString();
             beds.push(staff);
          }else{
            let tempStaff = new Staff(json['beds'][bednum]['staff_name'],json['beds'][bednum]['shift_type']);
            if(json['beds'][bednum]['alias'] == null)
              tempStaff.alias = tempStaff.staff_name;
            beds.push(tempStaff);
          }
      }
      allocation["beds"] = beds;
      for(let centralnum in json){
        if(centralnum != "beds" && json[centralnum]['staff_id'] != undefined){
          let staff:Staff =  this.findStaff(json[centralnum]['staff_id'], staffList);
          staff.shift_type = json[centralnum]['shift_type'].toString();;
          allocation[centralnum] = staff;
        }
        if(centralnum != "beds" && json[centralnum]['staff_name'] != undefined){
          allocation[centralnum] = new Staff(json[centralnum]['staff_name'],json[centralnum]['shift_type']);
          let tempStaff = new Staff(json[centralnum]['staff_name'],json[centralnum]['shift_type']);
            if(json[centralnum]['alias'] == null)
              tempStaff.alias = tempStaff.staff_name;
          allocation[centralnum] = tempStaff;
        }
      }

      return allocation as PodAllocation;
  }

  private findStaff(id:string, staffList:Staff[]):Staff{
    for(let i = 0 ; i < staffList.length ; i++){
        if(staffList[i]["id"] === id){
          return JSON.parse(JSON.stringify(staffList[i])) as Staff;
        }
      }
      return new Staff("");
  }
}

