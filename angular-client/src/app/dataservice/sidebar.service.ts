import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';  
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';

@Injectable()
export class SideBarService {
  private Url = 'api/team/';  // URL to web api

  constructor(private http: Http) {}

  getHttpAllocation(id: string): Promise<any> {
  return this.http.get(this.Url + id) 
    .toPromise()
    .then(response => this.format(response.json().team))
    .catch(this.handleError);
  }

  getHttpNurseById(id:String): Promise<any> {
  	return this.http.get("/api/staff/" + id)
  	.toPromise()
  	.then(response => response.json())
  	.catch(this.handleError);
  }

  putHttpAllocation(id: string, pod:any): Promise<any>{
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    const url = "/api/team/" + id;
    return this.http.put(url,JSON.stringify({team:this.reformat(pod)}), {headers: headers}) 
      .toPromise() 
      .then(()=>pod)
      .catch(this.handleError);
  }

   private reformat(allocation:any){
  	//replace name or id of nurse in beds
  	let bedIndex = Object.keys(allocation.beds);
  	for(var i = 1 ; i <= bedIndex.length; i++){
  		let shift_type = allocation.beds[i]["shift_type"];
  		let staff_name = allocation.beds[i]["staff_member"];

  		let id = this.findId(staff_name);
  		if(id){
  			allocation.beds[i] = {"staff_id":id,"shift_type":shift_type};
  		}else{
  			allocation.beds[i] = {"staff_name":staff_name,"shift_type":shift_type};
  		}	
  	}

  	 	//replace name or id of center staff
  	let centerIndex = Object.keys(allocation);
  	for(var j = 1; j<=6;j++){
  		let shift_type = allocation[centerIndex[j]]["shift_type"];
  		let staff_name = allocation[centerIndex[j]]["staff_member"];
 		let id = this.findId(staff_name);
 		if(id){
  			allocation[centerIndex[j]] = {"staff_id":id,"shift_type":shift_type};
  		}else{
  			allocation[centerIndex[j]] = {"staff_name":staff_name,"shift_type":shift_type};
  		}
  	}

  	return allocation;

  }

  //find wether the nurse is in database
  //if no return false
  //else return the nurse id 
  private findId(name:string){
  	return false;
  }

  //transform staff_id,staff_name to staff_member
  private format(allocation:any){
  	//replace name or id of nurse in beds
  	let bedIndex = Object.keys(allocation.beds);
  	for(var i = 1 ; i <= bedIndex.length; i++){
  		if("staff_id" in allocation.beds[i]){
  			let shift_type = allocation.beds[i]["shift_type"];
  			this.getHttpNurseById(i.toString()).then(nurse => 
  				allocation.beds[i] = {"staff_member": nurse.name, "shift_type": shift_type}
  			);
  		}else if("staff_name" in allocation.beds[i]) {
  			let staff_name = allocation.beds[i]["staff_name"];
  			let shift_type = allocation.beds[i]["shift_type"];
  			allocation.beds[i] = {"staff_member": staff_name, "shift_type": shift_type};
  		}else{
  			console.log("error");
  		}
  	}
  	//replace name or id of center staff
  	let centerIndex = Object.keys(allocation);
  	for(var j = 1; j<=6;j++){
  		if("staff_id" in allocation[centerIndex[j]]){
  			let shift_type = allocation[centerIndex[j]]["shift_type"];
  			this.getHttpNurseById(i.toString()).then(nurse => 
  				allocation[centerIndex[j]] = {"staff_member": nurse.name, "shift_type": shift_type}
  			);
  		}else if("staff_name" in allocation[centerIndex[j]]) {
  			let staff_name = allocation[centerIndex[j]]["staff_name"];
  			let shift_type = allocation[centerIndex[j]]["shift_type"];
  			allocation[centerIndex[j]] = {"staff_member": staff_name, "shift_type": shift_type};
  		}else{
  			console.log("error");
  		}
 
  	}
  	return allocation;
  }


  private handleError(error: any): Promise<any> {
  	console.error('An error occurred', error); // for demo purposes only
  	return Promise.reject(error.message || error);
  }

}