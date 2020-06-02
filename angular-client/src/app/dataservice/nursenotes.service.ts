import { Injectable } from '@angular/core';
import { Note } from './models/note';
import { Headers, Http } from '@angular/http';  
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';

@Injectable()
export class NoteService {
  private url = 'api/note';  // URL to web api

  constructor(protected http: Http) {
  }

  public getHttpNotes(staff_id:string): Promise<Note[]> {
    return this.http.get(this.url+"?staff_id="+staff_id)
      .toPromise()
      .then(response => this.noteListReviver(response.json()))
      .catch(this.handleError);
  }

  public postHttpNote(staff_id: string, contents: string): Promise<Note>{
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.post(this.url, {staff_id:staff_id, contents:contents}, {headers: headers})
      .toPromise()
      .then(response => this.noteConstructor(response.json()["note_id"],staff_id,contents))
      .catch(this.handleError);
  }

  public putHttpNote(note_id: string, contents: string): Promise<Note>{
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    return this.http.put(this.url +"/" + note_id,{contents:contents},{headers: headers})
      .toPromise()
      .then(response => this.noteReviver(response.json()))
      .catch(this.handleError);
  }

  // Delete note with id,
  public deleteHttpNote(note_id: string): Promise<boolean>{
    return this.http.delete(this.url + "/" + note_id)
      .toPromise()
      .then(response => response.json()["status"] == "success")
      .catch(this.handleError);
  }

  private handleError(error: any): Promise<any> {
    console.error('An error occurred', error); // for demo purposes only
    return Promise.reject(error.message || error);
  }

  private noteReviver(json:object):Note{
    if (json["note"]){ 
      return this.noteConstructor(json["note"]["note_id"], 
                                  json["note"]["staff_id"],
                                  json["note"]["contents"]);
    }
    return null;
  }

  private noteConstructor(note_id: string, 
                          staff_id: string, 
                          contents: string):Note {
    if(note_id && staff_id){
      return {note_id: note_id,
              staff_id: staff_id, 
              contents: contents} as Note;
    }
    return null;
  }

  private noteListReviver(json:object):Note[]{
    if(!json["note_list"]){
      return null;
    }
    let notes:Note[] = [];
    for(let item of json["note_list"]){
      let note:Note = this.noteConstructor(item["note_id"], 
                                           item["staff_id"], 
                                           item["contents"]);
      if(note){
        notes.push(note);
      }
    }
    return notes;
  }
}