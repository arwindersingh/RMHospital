import { Injectable } from '@angular/core';
import { Staff } from './models/staff';
import { Designation } from './models/designation';
import { Headers, Http } from '@angular/http';  
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';

@Injectable()
export class DesignationService {

 
  protected url = 'api/designation';  // URL to web api

  constructor(protected http: Http) {
    this.getDataObservable(this.url);   
  }

  getDataObservable(url:string) {
    return this.http.get(this.url)
        .map(data => {    
            return data.json();
    }).subscribe();
  }

  getHttpDesignation(): Promise<Designation[]> {
    return this.http.get(this.url)
      .toPromise()
      .then(response => this.designationsReviver(response.json()))
      .catch(this.handleError);
  }
  
  getHttpDesignationById(id): Promise<Designation> {
    return this.http.get(this.url+"/"+id)
      .toPromise()
      .then(response => this.designationReviver(response.json()) as Designation)
      .catch(this.handleError);
  }
  putHttpDesignation(designation): Promise<Designation>{
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    const url = "/api/designation/" + designation.designation_id;
    return this.http.put(url,JSON.stringify({"name" : designation.name,"designation_id" : designation.designation_id}), {headers: headers}) 
      .toPromise() 
      .then((response)=>this.designationReviver(response.json()))
      .catch(this.handleError);
  }
  postHttpDesignation(name:string): Promise<Designation>{
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    const url = "/api/designation/";
    return this.http.post(url,JSON.stringify({"name" : name}), {headers: headers}) 
      .toPromise() 
      .then(response=>this.typeConstructor(name, response.json()["designation_id"]))
      .catch(this.handleError);
  }
  deleteHttpDesignation(id:number):Promise<{result: boolean, reason: string}> {
   return this.http.delete("/api/designation/"+id)
      .toPromise()
      .then(response => {
        if (response.json()["status"] == "success"){
          return {result: true};
        } else {
          return {result: false, reason: response.json()["value"]["reason"]};
        }
      })
      .catch(this.handleError);
  }
  protected handleError(error: any): Promise<any> {
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }

  private designationsReviver(json:object){
    
    let designation_list:Designation[]=[];
    if(!json["designation_list"]){
      return []
    }
    for(let item of json["designation_list"]){
      let designation:Designation = this.typeConstructor(item["name"],item["designation_id"]);
      if(designation){
        designation_list.push(designation);
      }
    }
    return designation_list
  }
  private designationReviver(json:object){
    let designation:Designation;
    if(!json["designation"]){
      return;
    }
    else{
      designation = this.typeConstructor(json["designation"]["name"],json["designation"]["designation_id"]);

    }
    
    return designation;
  }
  private typeConstructor(name: string, id: number):Designation {
    if(name && id){
      return {name: name, designation_id: id} as Designation;
    }
    return null;
  }
}