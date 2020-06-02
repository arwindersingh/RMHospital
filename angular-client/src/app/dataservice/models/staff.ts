import { Note } from "./note";

export class Staff {
  staff_name : string;
  first_name: string;
  last_name: string;
  alias : string;
  shift_type : string;
  skills : string[];
  id : string;
  last_double : string;
  photo : number;
  staff_type : string;
  rosteron_id : string;
  notes : Note[];
  countOfTask: number;
  assignedPod: string;
  assignedBed: string[];

  constructor(firstname: string, shiftType: string = null, lastname: string=null, alias: string = null, staffType:string = null,
      id: string = null, photo: number = null, skills: string[] = [],
      last_double: string = null, rosteron_id: string = null,
      notes: Note[] = [], countOfTask:number = 0, assignedPod: string = "", assignedBed: string[] = []){
    this.first_name = firstname;
    this.last_name = lastname;
    if(this.last_name != null){  
      this.staff_name = this.first_name + " " + this.last_name;  
    }  
    else{  
      this.staff_name = this.first_name;  
    }  
    this.staff_name = this.first_name;  
    this.alias = alias;
    this.shift_type = shiftType;
    this.staff_type = staffType;
    this.last_double = last_double;
    this.skills = skills;
    this.id = id;
    this.photo = photo;
    this.rosteron_id = rosteron_id;
    this.notes = notes;
    this.countOfTask = countOfTask;
    this.assignedPod = assignedPod;
    this.assignedBed = assignedBed;
  }
}
