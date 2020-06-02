import { Injectable } from '@angular/core';
import { Skill } from './models/skill';
import { Headers, Http } from '@angular/http';  
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';

@Injectable()
export class SkillService {
  private url = 'api/skill/';  // URL to web api

  constructor(protected http: Http) {
    this.getDataObservable(this.url);   
  }

  public getDataObservable(url:string) {
    return this.http.get(this.url)
        .map(data => {    
            return data.json();
    }).subscribe();
  }

  public getHttpSkills(): Promise<Skill[]> {
    return this.http.get(this.url)
      .toPromise()
      .then(response => this.skillListReviver(response.json()))
      .catch(this.handleError);
  }

  public postHttpSkill(name: string): Promise<Skill>{
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post(this.url, {name:name}, {headers: headers})
      .toPromise()
      .then(response => this.skillConstructor(name, response.json()["skill_id"]))
      .catch(this.handleError);
  }

  public putHttpSkill(name: string, id: number): Promise<Skill>{
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.put(this.url + id,{name:name},{headers: headers})
      .toPromise()
      .then(response => this.skillReviver(response.json()))
      .catch(this.handleError);
  }

  // Delete skill with id id,
  // Won't delete skills which are associated with any staff
  public deleteHttpSkill(id: number): Promise<{result: boolean, reason: string}>{
    return this.http.delete(this.url + id)
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

  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }

  private skillListReviver(json:object):Skill[]{
    if(!json["skill_list"]){
      return null;
    }
    let skills:Skill[] = [];
    for(let item of json["skill_list"]){
      let skill:Skill = this.skillConstructor(item["name"], item["skill_id"]);
      if(skill){
        skills.push(skill);
      }
    }
    return skills;
  }

  private skillReviver(json:object):Skill{
    if (json["skill"]){ 
      return this.skillConstructor(json["skill"]["name"], json["skill"]["skill_id"]);
    }
    return null;
  }

  private skillConstructor(name: string, id: number):Skill {
    if(name && id){
      return {name: name, id: id} as Skill;
    }
    return null;
  }
}