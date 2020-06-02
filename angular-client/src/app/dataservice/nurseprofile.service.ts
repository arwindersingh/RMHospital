import { Injectable } from '@angular/core';
import { Staff } from './models/staff';
import { Headers, Http , RequestOptions, URLSearchParams} from '@angular/http';  
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { mockprofile } from './mockdata/mock-profile';


@Injectable()  
export class NurseProfileService {

  protected url = 'api/staff';  // URL to web api

  constructor(private http: Http) {
  }

getHttpAllStaff(): Promise<Staff[]> {
return this.http.get('api/staff')
.toPromise()
.then(response => this.staffListReviver(response.json()))
.catch(this.handleError);
}


getHttpStaffProfile(id:string): Promise<Staff> {
    return this.http.get('api/staff/' + id)
      .toPromise()
      .then(response => this.staffReviver(response.json(), id) as Staff)
      .catch(this.handleError);
  }


getHttpStaffByIds(ids: string[]): Promise<Staff[]>{
    let url = "api/staff/?";
    for(let i = 0; i < ids.length; i ++){
        url = url + "id=" + ids[i] +"&";
    }
    url = url.substring(0,url.length-1);
    return this.http.get(url)
    .toPromise()
    .then(response => this.staffListReviver(response.json()) as Staff[])
    .catch(this.handleError);
}



delete(id:string):Promise<any> {
   return this.http.delete("/api/staff/"+id)
      .toPromise()
      .then(response => response.json())
      .catch(this.handleError);
   
  }

 putHttpNurse(id:string, nurse: any): Promise<Staff>{
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    //for testing
   // console.log( JSON.stringify(this.nurseReplacer(nurse)));
    return this.http.put("/api/staff/" + id, JSON.stringify(this.nurseReplacer(nurse)), {headers: headers}) 
      .toPromise() 
      .then((response)=> this.staffReviver(response.json().staff, id) as Staff)
      .catch(this.handleError);
  }

postHttpNurse(nurse: any): Promise<any>{
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post("/api/staff/",  JSON.stringify(this.nurseReplacer(nurse)), {headers: headers}) 
      .toPromise() 
      .then(response => {
        let res = response.json();
        let id = -1;
        if(res["status"] == "success"){
          id = res["staff_id"];
        } 
        return id
      })
      .catch(this.handleError);
}

getHttpFiltedNurse(filter:any):Promise<any>{
  var options = "";
  let date = filter.pop().name;
  filter.push({"type":"double_before", "name": this.transTimestrap(date)});
  for(let f of filter){
    options = options + f.type + "=" + f.name + "&";
  }
  options = options.slice(0,-1);
  //api/staff?skills=ALS&&nursetype=ANUM&&lastdouble=2011-0-1
  return this.http.get('api/staff?'+options)
      .toPromise()
      .then(response => this.staffListReviver(response.json()))
      .catch(this.handleError);
  }

//get all last doubles of nurses
getNurseLastDouble(nurselist:any){
    let last_doubles = []
    for (let i = 0; i<nurselist.length; i++){
        if(last_doubles.indexOf(nurselist[i].last_double)==-1){
          last_doubles.push(nurselist[i].last_double);
        }else{
          //do nothing...
        }
    }
    return last_doubles;
  }

getRecentDouble(staffid:string): Promise<string[]>{
  let recent:string[] = [];
  return this.http.get('api/staff/'+staffid)
  .toPromise()
  .then(res=>{
      res.json()["staff"]["recent_doubles"]
      .forEach(timestrap=>{
        recent.push(this.transDate(timestrap));
      });
      return recent;
    })
  .catch(this.handleError);
}


// Convert between internal data structure and API JSON structure
private nurseReplacer(nurse){
    let nurseCopy = Object.assign({}, nurse);
    nurseCopy.name = nurse.staff_name;
    console.log( JSON.stringify(nurseCopy));
    delete nurseCopy.staff_name;
    if(nurse.last_double){
        let date = nurse.last_double.split('-');
        let selDate = {"year": date[0],"month": date[1],"day":date[2]}
        nurseCopy.last_double = this.transTimestrap(selDate);
    }
    if(nurseCopy["shift_type"]){
      delete nurseCopy.shift_type;
    }
    console.log( JSON.stringify(nurseCopy));
    return nurseCopy;
  }


private handleError(error: any): Promise<any> {
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }

private staffReviver(json:object, id:string): Staff{
      let staffCopy:Staff = Object.assign({}, json) as Staff;
      //staffCopy["staff_name"] = staffCopy["name"];
      staffCopy["staff_name"] = staffCopy["first_name"]+" "+staffCopy["last_name"];
      delete staffCopy["name"];
      staffCopy.id = id;
      staffCopy.last_double = this.transDate(staffCopy.last_double);
      return staffCopy;
  }

private staffListReviver(json:object): Staff[]{
    let nurses:Staff[] = [];
    for(let item of json["staff_list"]){
      nurses.push(this.staffReviver(item["staff"], item["id"]));
    }
    return nurses
}

  transTimestrap(selDate:any){
    //{year: 2017, month: 9, day: 7}
      let time1 = new Date();
      time1.setUTCFullYear(selDate.year);
      time1.setUTCMonth(selDate.month);
      time1.setUTCDate(selDate.day);
      time1.setUTCHours(0);
      time1.setUTCMinutes(0);
      time1.setUTCSeconds(0);
      time1.setUTCMilliseconds(0);
      let timestamp1 = Math.floor(time1.getTime() / 1000);

      // adjust the local time zone
      let time2 = new Date();
      let timestamp2 = time2.getTimezoneOffset()*60;
      let timestmp = timestamp1 + timestamp2;
      return timestmp
  }


  transDate(timestrap1:string){
    // adjust the local time zone
      if(!timestrap1){
        return null;
      }
      let timestrap = parseInt(timestrap1) - new Date().getTimezoneOffset()*60;
      let date = new Date(timestrap * 1000);
      let last_double = date.getFullYear().toString() + "-" + date.getMonth().toString()+ "-" + date.getDate().toString();
      return last_double;
  }

}