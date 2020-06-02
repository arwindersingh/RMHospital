import { Injectable } from '@angular/core';
import { Staff } from './models/staff';
import { Allocation } from './models/allocation';
import { Headers, Http } from '@angular/http';  
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';

@Injectable()
export class AllocationService {

  allocation : Allocation;

  protected url = 'api/team';  // URL to web api

  constructor(protected http: Http) {
    this.getDataObservable(this.url);   
  }

  getDataObservable(url:string) {
    return this.http.get(url)
        .map(data => {    
            return data.json();
    }).subscribe();
  }

  //stub
  getHttpAllocation(teamName: string): Promise<any> {return null}
  //stub
  getHttpNurse(id: string): Promise<any> {return null}
  //stub
  putHttpAllocation(name: string, alloc: any): Promise<any>{return null}

  protected handleError(error: any): Promise<any> {
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }

  SaveAllocation(teamName:string, allocation: any):void{
    
      
    this.putHttpAllocation(teamName,allocation);
  } 

}