import { Component, OnInit} from '@angular/core';
import { nurseList } from './mock';
import { PodAllocation } from '../dataservice/models/podallocation';
import { PodService } from '../dataservice/pod.service';
import { NurseProfileService } from '../dataservice/nurseprofile.service';
import { SideBarService } from '../dataservice/sidebar.service';
import { ImageService } from '../dataservice/image.service';
import { NoteService } from '../dataservice/nursenotes.service';
import { SkillService } from '../dataservice/skill.service';
import { Staff } from '../dataservice/models/staff';
import { Note } from '../dataservice/models/note';
import { sidebarStaff } from '../dataservice/models/sidebarStaff';
import * as Rx from 'rxjs/Rx'
import 'dragula/dist/dragula.css'

import {neighborBed} from '../dataservice/models/neighborBed';


@Component({
  selector: 'app-nurse-sidebar',
  templateUrl: './nurse-sidebar.component.html',
  styleUrls: ['./nurse-sidebar.component.css']
})
export class NurseSidebarComponent implements OnInit {

  poda: PodAllocation;
  podb: PodAllocation;
  podc: PodAllocation;
  podd: PodAllocation;

  //sidebarStaff has attribute tasks  
  //which save beds that have been assigned to this staff
  sidebarNurseList: sidebarStaff[] = []; 
  detailNurse:Staff;
  selectedBedId:string;
  teamName = ['A','B','C','D']; 
  selectedBed:Staff;
  rosteredNurseList : string[]=[];
  // total number of skills
  numSkills:number;
  selectedRecentDouble:string[] =[];
  //used for search funtion 
  search="";  //search by name
  search2 = "";  //search by type
  search3 = "";  //search by skill


  selectedNurse: Staff;

  neighborSet: Array<neighborBed>;
  isSelf:boolean = false;

  warnMessage: string = "";

  constructor(
    private podService: PodService,
    private nurseProfileService: NurseProfileService,
    private imageService: ImageService,
    private noteService: NoteService,
    private skillService: SkillService
  ) {} 

  ngOnInit() {
      Rx.Observable.forkJoin(
        this.nurseProfileService.getHttpAllStaff().then(
          nurseList=>nurseList.forEach(
            staff=>this.sidebarNurseList.push(new sidebarStaff(staff))
          )
        ),
        this.podService.getHttpAllocation("a").then(pod=>this.poda = pod),
        this.podService.getHttpAllocation("b").then(pod=>this.podb = pod),
        this.podService.getHttpAllocation("c").then(pod=>this.podc = pod),
        this.podService.getHttpAllocation("d").then(pod=>this.podd = pod),
        this.skillService.getHttpSkills().then(skills=>this.numSkills = skills.length)
      ).toPromise().then(_ => {
          //initially, assign poda 1 to edit card
          this.selectedBed = this.poda.beds[0];
          this.selectedBedId = "A1";
          //create sidebarStaff list
          //assign tasks to sidebarStaff bases on current pod allocation
          this.assignTasks();
          //Init neighborSet
          this.neighborSet = neighborBed.initNeighborSet();
      });

  }

  //the staff info will be show in detail window
  showDetailNurse(nurse:Staff){
    this.getNotes(nurse);
    this.nurseProfileService.getRecentDouble(nurse.id)
          .then(res=>this.selectedRecentDouble = res);
    this.detailNurse = nurse;
  }

  // Get notes for a staff member and add them
  getNotes(nurse:Staff){
    if (!nurse['id']) return;
    this.noteService.getHttpNotes(nurse.id).then( notes => {
      if (notes.length != 0){
        nurse.notes = notes
      } else {
        // Make a new empty nurse for this nurse
        nurse.notes = [new Note];
      }
    });
  }

  autosaveAllocation()
  {
    this.podService.putHttpSidebar("a",this.transfer(this.poda));
    this.podService.putHttpSidebar("b",this.transfer(this.podb));
    this.podService.putHttpSidebar("c",this.transfer(this.podc));
    this.podService.putHttpSidebar("d",this.transfer(this.podd));
  }

  //save the bed info and staff has been assigned to this bed
  //if the bed is empty, staff.name will be ""
  clickCell(staff:Staff,bedid:string){
    this.getNotes(staff);
    this.selectedBed = staff;
    this.selectedBedId = bedid;
    console.log("clickCell: " + staff.staff_name + "  bed id: " + bedid + " countOfTaske: " + staff.countOfTask);

    //ignore added one(myself) and it need to assume that adding one to test if more than 2 bed
    if(this.selectedNurse != null){
      if(this.selectedNurse.assignedBed.indexOf(bedid) == -1){
        this.selectedNurse.countOfTask++;
        this.selectedNurse.assignedBed.push(bedid);
        this.isSelf = false;
        console.log("countOfTask: not assign to myself! assignedBed: " 
          + this.selectedNurse.assignedBed + " selected bed " + bedid + 
          " countOfTask: " + this.selectedNurse.countOfTask);
      }
      else{
        this.isSelf = true;
      }
      console.log("click cell selected nurse countOfTask " + this.selectedNurse.countOfTask);
      this.changeBedAllocation(this.selectedNurse);
      this.selectedNurse = null;
    }
  }

  //find out whether it is neighbor or not
  private detectIsNeighbor(selectedNurse:Staff, neighborBedId:string):boolean{
    
    for(var i = 0; i < selectedNurse.assignedBed.length; i++){
      for(var j = 0; j < this.neighborSet.length; j++){
        if(selectedNurse.assignedBed[i] == this.neighborSet[j].ownBedId){
          for(var k = 0; k < this.neighborSet[j].neighborBedId.length; k++){
            if(neighborBedId == this.neighborSet[j].neighborBedId[k]){
              return true;
            }
          }
        }
      }
    }

    return false;
  }

  //change staff of current selected bed
  changeBedAllocation(nurse:Staff){
    console.log("Nurse name: " + nurse.staff_name);
    var result = true;
    if(this.selectedBed)
    { //if has a selected bed
      let podid = this.selectedBedId.slice(0,1);
      let bedid = this.selectedBedId.slice(1,);
      //nurse will be assigned to current selected bed
      let addBeds:Staff = nurse;
      this.getNotes(addBeds);

      // At first, check if the nurse is assigned to different pods.
      // If the nurse is assigned to same pods.
      if(addBeds.assignedPod == podid && !this.isSelf){  
        // Check if there is a same bed

        // check if the nurse is assigned to more than two beds.
        if(addBeds.countOfTask >2 && !this.isSelf){
          result = confirm("This nurse is assigned to more than 2 beds, would you like to continue?");
          this.warnMessage += (addBeds.staff_name + " has been assigned to more than 2 beds!\n");
          if(result == false){
            this.deleteBed(this.selectedBed);
          }else{
            // if the user keep assigning the nurse to the beds even if the nurse
            // is already assigned to more than two beds. Then check if the nurse is 
            // assigned to the neighbour bed. 
            if(addBeds.countOfTask > 1 && bedid.length < 3 && !this.isSelf){
              if(!this.detectIsNeighbor(addBeds, this.selectedBedId)){
                result = confirm("This nurse isn't assigned to adjust beds, would you like to continue?");
                this.warnMessage += (addBeds.staff_name + " isn't assigned to adjust beds!\n");
                if(result == false){
                  this.deleteBed(this.selectedBed);
                }else{
                  alert("Add Successfully")
                }
              }
            }
          }
        // if the nurse is not assigned to more than two beds.
        }else{
          // check if the nurse is assgined to the neighbour bed. 
          if(addBeds.countOfTask > 1 && bedid.length < 3 && !this.isSelf){
            if(!this.detectIsNeighbor(addBeds, this.selectedBedId)){
              result = confirm("This nurse isn't assigned to adjust beds, would you like to continue?");
              this.warnMessage += (addBeds.staff_name + " isn't assigned to adjust beds!\n");
              if(result== false){
                this.deleteBed(this.selectedBed);
              }else{
                alert("Add Successfully")
              }
            }
          }
        }
      // if the nurse is not assign to any Pod before, just set the assignedPod of the
      // nurse to the current pod.
      }else if(addBeds.assignedPod == "" && !this.isSelf){
        addBeds.assignedPod = podid;
      // if the nurse is assigned to different pods, pops up a warning message.
      }else if(addBeds.assignedPod != podid && !this.isSelf){
        result = confirm("This nurse is assigned to different pods, would you like to continue?");
        this.warnMessage += (addBeds.staff_name + " has been assigned to differn pods!\n")
        if(result == false){
          this.deleteBed(this.selectedBed);
        // if the user keeps assign the nurse to different pods.
        }else{
          // check if the nurse is assigned to more than two beds.
          if(addBeds.countOfTask >2 && !this.isSelf){
            result = confirm("This nurse is assigned to more than 2 beds, would you like to continue?");
            this.warnMessage += (addBeds.staff_name + " has been assigned to more than 2 beds!\n");
            if(result==false){
              this.deleteBed(this.selectedBed);
            // if the nurse is already assigned to more than two beds, but the user 
            // stills want to assign the nurse to the bed.
            }else{
              // check if the nurse is assigned to the neighbour bed. 
              if(addBeds.countOfTask > 1 && bedid.length < 3 && !this.isSelf){
                if(!this.detectIsNeighbor(addBeds, this.selectedBedId)){
                  result = confirm("This nurse isn't assigned to adjust beds, would you like to continue?");
                  this.warnMessage += (addBeds.staff_name + " isn't assigned to adjust beds!\n");
                  if(result == false){
                    this.deleteBed(this.selectedBed);
                  }else{
                    alert("Add Successfully")
                  }
                }
              }
            }
          // if the nurse is not assigned to more than two beds, still need to check 
          // if the nurse is assigned to the neighbour bed.
          }else{
            if(addBeds.countOfTask > 1 && bedid.length < 3 && !this.isSelf){
              if(!this.detectIsNeighbor(addBeds, this.selectedBedId)){
                result = confirm("This nurse isn't assigned to adjust beds, would you like to continue?");
                this.warnMessage += (addBeds.staff_name + " isn't assigned to adjust beds!\n");
                if(result == false){
                  this.deleteBed(this.selectedBed);
                }else{
                  alert("Add Successfully")
                }
              }
            }
          }
        }
      }    

      

      if(podid == "A")
      {
        if(bedid.length<3){
           //assign to poda bed list
           this.poda.beds[parseInt(bedid)-1] = addBeds ;
        }else{
           //assign to poda central staffs
          this.poda[bedid] =  addBeds ;
        }
      }
      else if(podid == "B"){
        if(bedid.length<3){
          //assign to podb bed list
          this.podb.beds[parseInt(bedid)-1] = addBeds ;
        }else{
          //assign to podb central staffs
          this.podb[bedid] =  addBeds ;
        }
        
      }
      else if(podid == "C"){
        if(bedid.length<3){
          //assign to podc bed list
          this.podc.beds[parseInt(bedid)-1] = addBeds ;
        }else{
          //assign to podc central staffs
          this.podc[bedid] =  addBeds ;
        }
      }  else if(podid == "D"){
        if(bedid.length<3){
          //assign to podc bed list
          this.podd.beds[parseInt(bedid)-1] = addBeds ;
        }else{
          //assign to podc central staffs
          this.podd[bedid] =  addBeds ;
        }
      }
      //the staff at seleted bed has became addBeds 
      this.selectedBed =  addBeds;
      //assign task to sidebar staff based on new allocation
      this.assignTasks();

      console.log("selectedBed: " + this.selectedBed.assignedBed + " countOfTask: " + this.selectedBed.countOfTask);

      if(result == false)
      {
          this.deleteBed(this.selectedBed);
          result = true;
      }
    }
    else
    {
      alert("Please select a bed first.");
    }
  }

  //delete staff at position
  deleteBed(position:any){
    let emptyStaff = new Staff("","8");

    // decrease selected bed(nurse staff) task count
    this.selectedBed.countOfTask--;
    // remove the bed from current bed's staff
    if(this.selectedBed.assignedBed.indexOf(this.selectedBedId) != -1)
    {
      this.selectedBed.assignedBed.splice(this.selectedBed.assignedBed.indexOf(this.selectedBedId),1);
    }
    console.log("delete selectBed assignBed: " + this.selectedBedId + " right now: " + this.selectedBed.assignedBed);

    if(this.selectedBed.countOfTask == 0){
      this.selectedBed.assignedPod ="";
      console.log(" No beds is assigned on the nurse")
    // if the nurse is assigned on more than two beds in the pod then
    // the assigned pod of the nurse would not need to change 
    } else{
      console.log( "assigned pod of the nurse: "+this.selectedBed.assignedPod);
    }

    this.selectedBed = emptyStaff;
    this.changeBedAllocation(emptyStaff);
  }

  rosteredNurses(nurses:string[]){
    this.rosteredNurseList = nurses;
    console.log(nurses);
    this.assignTasks();
  }

  //save current allocation
  saveAllocation(){
    let a = confirm("Do you want to save?")
    if(a){
    this.podService.putHttpSidebar("a",this.transfer(this.poda));
    this.podService.putHttpSidebar("b",this.transfer(this.podb));
    this.podService.putHttpSidebar("c",this.transfer(this.podc));
    this.podService.putHttpSidebar("d",this.transfer(this.podd));
    alert("Saved successfully");
    }
  }

  selectNurse(nurse: Staff){
    console.log("Nurse name: " + nurse.staff_name);
    this.selectedNurse = nurse;
    // Notice that a same nurse has been assigned more than two beds

  }

   //get a staff by id
  private getNurse(id:string): sidebarStaff{
      if(this.sidebarNurseList){
        for(let i = 0; i < this.sidebarNurseList.length;i++){
          if(this.sidebarNurseList[i].id == id){
            return this.sidebarNurseList[i];
          }
        }
      }
      return null;
  }

  //assign tasks to sidabarnurse list based on podid and pod
  // Also does note requests for each nurse of the allocation
  private addTasktoStaff(podid:string, pod:PodAllocation){
      for(let i = 0 ; i < pod.beds.length ; i++){
        let id = pod.beds[i].id;
        if(id != null){
          let staff = this.getNurse(id);
          if(staff != null){
            staff.shift_type = pod.beds[i].shift_type
            pod.beds[i] = staff
            staff.tasks.push(podid+(i+1).toString());
            this.getNotes(pod.beds[i]);

            //set staff attributes when init
            staff.assignedPod = podid;
            //ignore added one and reload assigned bed when refresh or change allocation
            if(staff.assignedBed.indexOf(podid+(i+1).toString()) == -1)
              staff.assignedBed.push(podid+(i+1).toString());
            staff.countOfTask = staff.tasks.length;
          }
        }
      }
      for(let centralnum in pod){
        if(centralnum != "beds"){
            let id = pod[centralnum].id;
             if(id != null){
              let staff = this.getNurse(id);
              if(staff != null){
                staff.shift_type = pod[centralnum].shift_type
                pod[centralnum] = staff
                staff.tasks.push(podid+centralnum);
                this.getNotes(pod[centralnum]);
                
                //set staff attributes when init
                staff.assignedPod = podid;
                //ignore added one and added to staff
                if(staff.assignedBed.indexOf(podid+centralnum) == -1)
                  staff.assignedBed.push(podid+centralnum);
                staff.countOfTask = staff.tasks.length;
              }
            }
        }
      }
  }



  //assign tasks to sidabarnurse list
  private assignTasks(){
     //empty current tasks
     this.sidebarNurseList.forEach(nurse=>nurse.tasks=[]);
     //reassign tasks based on current allocation
     this.addTasktoStaff("A",this.poda);
     this.addTasktoStaff("B",this.podb);
     this.addTasktoStaff("C",this.podc);
     this.addTasktoStaff("D",this.podd);

     //sort by number of tasks and rosteredNurse
     //top: rosteredNurse, middle: nurse with tasks, bottom: other nurses
    this.sidebarNurseList.sort((a:sidebarStaff,b:sidebarStaff)=>{
       // if both rosteredNurse, one with more tasks bottom
      if(this.isRost(a)==2 && this.isRost(b)==2){ 
         return a.tasks.length-b.tasks.length;
       // if one of them is rosteredNurse, on the top
       }else if(this.isRost(a)==2 && !(this.isRost(b)==2)){
         return -1;
       //if one of them is rosteredNurse, on the top
       }else if(!(this.isRost(a)==2) && this.isRost(b)==2){
         return 1;
       // if both not rosteredNurse, one with more taks on top
       }else{
         return b.tasks.length-a.tasks.length;
       }
     });
  }

  //transfer the pod allocation to correct format for http put
  private transfer(pod:PodAllocation){
    let alloc: object = {};
    let beds: object = {};

    for(let i = 0; i<pod.beds.length;i++){
      if(pod.beds[i].id){
        beds[(i+1).toString()] = {"staff_id": pod.beds[i].id,"shift_type":pod.beds[i].shift_type};

      }else{
        beds[(i+1).toString()] = {"staff_name": pod.beds[i].staff_name,"shift_type":pod.beds[i].shift_type};
      }
    }
    alloc["beds"] = beds;    
    for(let centralnum in pod){
        if(centralnum != "beds" && pod[centralnum].id!=null){
          alloc[centralnum] = {"staff_id": pod[centralnum].id,"shift_type":pod[centralnum].shift_type};
        }
        else if(centralnum != "beds"){
         alloc[centralnum] = {"staff_name":  pod[centralnum].staff_name,"shift_type": pod[centralnum].shift_type};
        }
        
      }
      return alloc;
  }

  // Check if nurse is included in csv file
  // 0: no file is uploaded
  // 1: staff not in file
  // 2: staff rostered on in file
  private isRost(nurse:sidebarStaff):number{
    if (this.rosteredNurseList.length == 0){
      return 0;
    }
    for(let i = 0; i < this.rosteredNurseList.length;i++){
        if(this.rosteredNurseList[i] == nurse.rosteron_id){
          return 2;
        }
    }
    return 1;
  }

  // Take a list of skill names and return a commar space
  // seperated list of them
  private skillsToStr(skills: string[]):string {
    if (!skills){
      return '';
    }
    if (skills.length == this.numSkills) {
      return 'All skills';
    } else {
      return skills.join(', ');
    }
  }

  private saveNote(noteData: {id: string, note: Note}){
    if(noteData.note.note_id){
      this.noteService.putHttpNote(noteData.note.note_id, noteData.note.contents).then(note => {
        alert("Saved note");
      });
    } else {
      this.noteService.postHttpNote(noteData.id, noteData.note.contents).then(note => {
        this.selectedBed.notes[0] = noteData.note;
        alert("Saved note");
      });
    }
  }
}