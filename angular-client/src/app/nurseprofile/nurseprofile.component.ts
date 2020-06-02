import { ViewChild } from '@angular/core';
import { Component, OnInit, Input } from '@angular/core';
import { IMyDpOptions } from 'mydatepicker';

import { ImageService } from '../dataservice/image.service';
import { NurseProfileService } from '../dataservice/nurseprofile.service';
import { SkillService } from '../dataservice/skill.service';

import { DesignationService } from '../dataservice/designation.service';
import { NgZone } from '@angular/core';

import { Staff } from '../dataservice/models/staff';
import { Skill } from '../dataservice/models/skill';
import { NoteService } from '../dataservice/nursenotes.service';
import { Note } from '../dataservice/models/note';
import { Designation } from '../dataservice/models/designation';
interface nurseDict {
  [id: string] : Staff;
}

@Component({
  selector: 'app-nurseprofile',
  templateUrl: './nurseprofile.component.html',
  styleUrls: ['./nurseprofile.component.css']
})
export class NurseprofileComponent implements OnInit {

  @ViewChild('editPhotoUpload')
  private editPhotoUploadForm: any;

  @ViewChild('createPhotoUpload')
  private createPhotoUploadForm: any;

  nurses:nurseDict = {};
  check:boolean = true;
  length:Number;

  nurseType: string;
  selectednurse:Staff;
  selectedNotes:Note[] = [new Note(), new Note()];
  selectedRecentDouble:string[] =[];

  // Currently edited nurse information -- would be better as a contained object
  editnurse:Staff;
  editNurseImage: File;
  editNotes: Note[] = [new Note(), new Note()];
  editNurseImageUrl: string;
  editeNurseImageUpdated: boolean = false;
  editEnabled:boolean = false;

  newnurse:Staff;

  nurseTypes:Designation[];
  skillTypes:Skill[];
  skillFilter:object[];
  typeFilter:object[];
  doubleFilter:object[];
  editnurse_skill_check = [];
  edit:boolean = false;
  moreFilter:boolean = false;
  search:string = '';
  filter = [];
  quickFilter = [
    {
      "name": "condition1",
      "select": false,
    },
    {
      "name": "condition2",
      "select": false,
    },
    {
      "name": "condition3",
      "select": false,
    }
  ];

  myDatePickerOptions: IMyDpOptions = {
        // other options...
        dateFormat: 'dd.mm.yyyy',
        showClearDateBtn :false
  };

  // Nurse creation information -- would be better as a contained object
  // Initialized to specific date (09.10.2018).
  model: any = { date: { } };
  private model_0: any =  { date: {year: 0, month: 0, day: 0}  };
  private createNurseImage: File;
  private createNurseImageUrl: string;
  private createNurseImageUpdated: boolean = false;


  constructor(private nurseprofileService: NurseProfileService,
    private skillService: SkillService,
    private noteService: NoteService,
    private designationService: DesignationService,
    private imageService: ImageService,
    private zone:NgZone) { }


  ngOnInit() {
    this.nurseprofileService.getHttpAllStaff().then(staff_list => {
      staff_list.forEach(s => this.nurses[s.id] = s)
    });
    this.skillService.getHttpSkills().then(skills => {
      this.skillTypes = skills;
      this.skillFilter = this.getFilter(this.skillTypes.map(s => s.name));
    });
     this.designationService.getHttpDesignation().then(type => {
       this.nurseTypes = type;
       this.typeFilter = this.getFilter(this.nurseTypes.map(s => s.name));
     });
     var dataobj =  new Date();
     this.model_0.date.year  = dataobj.getFullYear();
     this.model_0.date.month = dataobj.getMonth() + 1;
     this.model_0.date.day   = dataobj.getDate();

     this.newnurse = new Staff(
       null,
       null,
       null,
       null,
       // null, // shift_type
        null, //staff_type
        null, //id
        null, //photo
        null, //skills
        null // last_double
     );

  }

  // When click a nurse profile, save this nurse info to selectedurse
  // and editnurse class variable.
  private selectNurse(nurse:Staff){
    this.selectednurse = nurse;
    this.editnurse = JSON.parse(JSON.stringify(nurse))

    for(let i = 0; i < this.skillTypes.length; i++){
      if(this.editnurse.skills.indexOf(this.skillTypes[i].name) != -1){
          this.editnurse_skill_check[i] = {"name":this.skillTypes[i].name,"check":true}
      }else{
        this.editnurse_skill_check[i] = {"name":this.skillTypes[i].name,"check":false};
      }
    }
    if(this.selectednurse.last_double){
      let date = this.selectednurse.last_double.split('-');
      this.model.date.year = Number(date[0]);
      this.model.date.month = Number(date[1]);
      this.model.date.day = Number(date[2]);
    }
    else{
      this.model.date.year = 0;
      this.model.date.month = 0;
      this.model.date.day = 0;
    }
    this.noteService.getHttpNotes(this.selectednurse.id).then(res =>{
      if(res && res.length > 0){
        this.selectedNotes[0] = res[0];
        this.editNotes[0] = JSON.parse(JSON.stringify(res[0]));
      }else{
        this.editNotes[0] = new Note();
        this.selectedNotes[0] = new Note();
      }
      if(res && res.length > 1) {
          this.selectedNotes[1] = res[1];
          this.editNotes[1] = JSON.parse(JSON.stringify(res[1]));
      }else{
        this.editNotes[1] = new Note();
        this.selectedNotes[1] = new Note();
      }
    });

    this.nurseprofileService.getRecentDouble(this.selectednurse.id)
    .then(res=>this.selectedRecentDouble = res);
  }

  //transform to filter {"name": "condition1","select": false,}
  private getFilter(skills:Array<string>){
      let Filters = [];
      let f:object;
      for(let i = 0; i<skills.length; i++){
        f = {"name": skills[i],"select": false,}
        Filters.push(f);
      }
      return Filters;
  }

  //submit the filter, get the new list of nurses
  subFilter(){
    for(let i=0;i<this.quickFilter.length;i++){
      if(this.quickFilter[i]["select"]===true){
        this.filter.push({"type":"quick","name":this.quickFilter[i]["name"]});
      }
    }
    for(let i=0;i<this.skillFilter.length;i++){
      if(this.skillFilter[i]["select"]===true){
        this.filter.push({"type":"skill","name":this.skillFilter[i]["name"]});

      }
    }
    for(let i=0;i<this.typeFilter.length;i++){
      if(this.typeFilter[i]["select"]===true){
        this.filter.push({"type":"type","name":this.typeFilter[i]["name"]});
      }
    }
    this.filter.push({"type":"double_before","name":this.model_0.date});
    this.nurseprofileService.getHttpFiltedNurse(this.filter)
      .then(staff_list => {
        this.nurses = {};
        staff_list.forEach(s => this.nurses[s.id] = s)

       });
      this.filter =[];
  }


  //show or hiden the more filter part
  private showMoreFilter(){
    this.moreFilter = !this.moreFilter;
  }


  //save the edited nurse info
  private saveEdit(ngForm:any){
    var c = confirm("Do you want to save?");
    if(c){
      let skills=[];
      let date = null;
      if(this.model.date.year!=0 && this.model.date.month!=0 && this.model.date.day!=0){
        date = this.model.date.year+'-'+this.model.date.month+'-'+this.model.date.day;
      }
      let skillList = [];
      for(let i =0; i<this.editnurse_skill_check.length; i++){
          if(this.editnurse_skill_check[i].check == true){
            skillList.push(this.editnurse_skill_check[i].name);
          }
      }
      this.editnurse.skills = skillList;
      this.editnurse.last_double = date;

      // If an image is present for upload, we need to upload it
      if (this.editNurseImage) {
        // Otherwise we will create a new photo and use that
        this.imageService.uploadImage(this.editNurseImage)
          .then(imageId => {
            // If the nurse already had a photo, we delete it
            // But we need to delete the photo after the nurse is updated
            if (this.editnurse.photo) {
              let oldPhotoId = this.editnurse.photo;
              this.editnurse.photo = imageId;
              this.updateNurse(this.editnurse.id, this.editnurse)
                .then(() => {
                  this.imageService.deleteImage(oldPhotoId);
                  this.tearDownEdit();
                });
              this.updateNote();
              return;
            }
            this.editnurse.photo = imageId;
            this.updateNurse(this.editnurse.id, this.editnurse)
              .then(() => { this.tearDownEdit(); });
            this.updateNote();
          });
        return;
      }

      this.updateNurse(this.editnurse.id, this.editnurse)
        .then(() => {
          this.tearDownEdit();
          this.nurseprofileService.getRecentDouble(this.selectednurse.id)
          .then(res=>this.selectedRecentDouble = res);
        });
      this.updateNote();

    }


  }

  // Save current edited notes to API
  private updateNote(){
    let promisePost:Promise<any> = undefined;
    for(let i in [0,1]){
      if(this.editNotes[i].contents == this.selectedNotes[i].contents)
        continue;
      if(this.editNotes[i].note_id){
        this.noteService.putHttpNote(
          this.editNotes[i].note_id,
          this.editNotes[i].contents
        );
      } else {
        if (promisePost) {
          promisePost.then(_ => {
            this.noteService.postHttpNote(this.editnurse.id,this.editNotes[i].contents).then(
              note => this.editNotes[i] = note
            );
          });
        } else {
          promisePost = this.noteService.postHttpNote(this.editnurse.id,this.editNotes[i].contents).then(
            note => this.editNotes[i] = note
          );
        }
      }
      this.selectedNotes[i] = this.editNotes[i];
    }
  }

  private updateNurse(id: string, nurse: Staff): Promise<any> {
    return this.nurseprofileService.putHttpNurse(id, nurse)
      .then(updatedNurse =>  {
        this.nurses[id] = updatedNurse;
        this.selectednurse = updatedNurse;
      });
  }

  //opreation after clicking "new"
  private onAdd(){
    this.model.date.year = 0;
    this.model.date.month = 0;
    this.model.date.day = 0;

  }

  //Create a new nurse
  saveAdd(ngForm:any){
    //ngForm.value.alias = (document.getElementById('alias')).value;
    console.log(ngForm.value.alias);
    let skillList : string[] = [];
    for(let i=0; i<this.skillTypes.length; i++){
      let skill = this.skillTypes[i].name

      if(ngForm.value[skill] == true){
        skillList.push(skill);
      }

    }
    let date = ngForm.value.mydate.formatted;
    if(date){
        date = date.split('.');
        date = date[2]+'-'+date[1]+'-'+date[0];
    }

    var c = confirm("Do you want to create?");
    if(c) {
    //  var fullname = ngForm.value.firstName+"#"+ngForm.value.lastName;
      // If a photo has been added, we need to do that first
      if (this.createNurseImage) {
        // First upload the photo
        this.imageService.uploadImage(this.createNurseImage)
          .then(imageId => {
            // Now create the nurse
            let nurse = new Staff(
              ngForm.value.firstName,
              null,
              ngForm.value.lastName,
              ngForm.value.alias,
            //  null,
              ngForm.value.typeFilter,
              null,
              imageId,
              skillList,
              date);
            // Now upload the nurse
            this.nurseprofileService.postHttpNurse(nurse)
              .then(id => {
                if (id > 0) {
                  nurse.id = id;
                  this.nurses[id] = nurse;
                  return;
                }

                // If we get back a bad nurse ID, we notify the user
                alert('Could not save nurse {' + nurse.first_name + '} to database. Check connection');
              });
          });

          return;
      }else{

        let nurse = new Staff(
         // fullname, // staff_name
         ngForm.value.firstName,
         null,
         ngForm.value.lastName,
          ngForm.value.alias,
         // null, // shift_type
          ngForm.value.typeFilter, //staff_type
          null, //id
          null, //photo
          skillList, //skills
          date // last_double
        )
        this.nurseprofileService.postHttpNurse(nurse)
        .then(id=>{
          if(id > 0){
              nurse.id = id;
              this.nurses[nurse.id] = nurse;
          } else {
            alert("Could not save " + nurse.first_name + " to database. Check connection.");
          }
        });
        //output the nurse information to console
        console.log(nurse);
      }
    }
    // If not reloading, there is a bug here.
    location.reload();
  }

  private editNurse(buttonID: string){
    let password = prompt("Please enter password");
    if(password == "icuaccess"){
       this.editEnabled = true;
       // click the button as a callback, after ngIf creates it
       setTimeout(() => document.getElementById(buttonID).click(), 0);
    }else{
      alert("Incorrect password");
    }
   }

  //delete the selected nurse
  private deleteNurse(){
   let c = confirm("Do you want to delete this nurse?");
   if(c){
      this.nurseprofileService.delete(this.selectednurse.id)
      .then(staff=>{
        delete this.nurses[this.selectednurse.id]
      });
     }
   }

  private nursesToArray(nursesObject):Staff[] {
    let nurses:Staff[] = [];
    for (let id in nursesObject){
      if (nursesObject.hasOwnProperty(id)) {
        nurses.push(nursesObject[id]);
      }
    }
    return nurses;
  }

  // Take a list of skill names and return a commar space
  // seperated list of them
  private skillsToStr(skills: string[]):string {
    if (!skills || !this.skillTypes){
      return '';
    }
    if (skills.length == this.skillTypes.length) {
      return 'All skills';
    } else {
      return skills.join(', ');
    }
  }

  updateSkills(newSkills: {skills: Skill[], updatedSkill: Skill}) {
    if(!newSkills){
      return;
    }
    // Check a Staff to see if they have oldName and update it if they do
    let updateNurseSkill = (nurse : Staff, oldName: string, updatedSkill : Skill) => {
      let newSkills:string[] = [];
      for (let i in nurse.skills){
        if (nurse.skills[i] == oldName){
          newSkills.push(updatedSkill.name);
        } else {
          newSkills.push(nurse.skills[i]);
        }
      }
      nurse.skills = newSkills;
    }
    // Check and update all Staff which have the old skill name
    if (newSkills["updatedSkill"]){
      let oldName = this.skillTypes.filter(s => s.id == newSkills.updatedSkill.id)[0].name;
      for(let id in this.nurses){
        updateNurseSkill(this.nurses[id], oldName, newSkills.updatedSkill);
      }
      updateNurseSkill(this.editnurse, oldName, newSkills.updatedSkill);
      updateNurseSkill(this.selectednurse, oldName, newSkills.updatedSkill);
    }
    // Update all skill lists and check boxes
    if (newSkills["skills"]){
      this.skillTypes = newSkills.skills;
      this.skillFilter = this.getFilter(this.skillTypes.map(s => s.name));
      for(let i = 0; i < this.skillTypes.length; i++){
        if(this.editnurse.skills.indexOf(this.skillTypes[i].name) != -1){
            this.editnurse_skill_check[i] = {"name":this.skillTypes[i].name,"check":true}
        }else{
          this.editnurse_skill_check[i] = {"name":this.skillTypes[i].name,"check":false};
        }
      }
    }
  }

  /**
   * Get a URL for the image of the given staff member
   * @param staff the staff member whose image we want
   */
  private getNurseImageUrl(staff: Staff) {
    // If an image is currently set but not fully uploaded, point a URL to the object in memory itself
    if (this.editNurseImage && staff.id == this.editnurse.id) {

      // If we don't have a URL yet, or need to update it, generate a new URL
      if (!this.editNurseImageUrl || this.editeNurseImageUpdated) {
        let reader: FileReader = new FileReader();
        reader.addEventListener('load', () => {
          this.editNurseImageUrl = reader.result;
        });
        reader.readAsDataURL(this.editNurseImage);
        this.editeNurseImageUpdated = false;
      }

      return this.editNurseImageUrl;
    }

    // Otherwise get the service to deal with things
    let url =  this.imageService.getImageUrlFromId(staff && staff.photo || null);
    return url;
  }

  /**
   * Get the image uploaded to the create nurse form
   */
  getCreateFormImageUrl() {
    // If an image has been set but not yet uploaded
    if (this.createNurseImage) {
      // If there is not already a URL or it needs updating, create a new one
      if (!this.createNurseImageUrl || this.createNurseImageUpdated) {
        let reader: FileReader = new FileReader();
        reader.addEventListener('load', () => {
          this.createNurseImageUrl = reader.result;
        });
        reader.readAsDataURL(this.createNurseImage);
        this.createNurseImageUpdated = false;
      }

      return this.createNurseImageUrl;
    }

    // If no image is uploaded, use the image service's default
    return this.imageService.getImageUrlFromId(null);
  }

  /**
   * When nurse edit images are added, set the image file to hold it
   * @param event the image set event
   */
  private onEditImageAdd(event) {
    let file: File = event.target.files.item(0);
    this.editNurseImage = file;
    this.editeNurseImageUpdated = true;
  }

  /**
   * When nurse creation modal has an image added, we set the internal file to hold it
   * @param event the image set event
   */
  onCreateImageAdd(event) {
    let file: File = event.target.files.item(0);
    this.createNurseImage = file;
    this.createNurseImageUpdated = true;
  }

  /**
   * Make sure we reset all information when the edit modal is exited
   */
  private tearDownEdit() {
    this.editNurseImage = null;
    this.editNurseImageUrl = null;
    this.editPhotoUploadForm.nativeElement.value = '';
  }

  /**
   * Make sure we reset all information when the create modal is exited
   */
  private tearDownCreate() {
    this.createNurseImage = null;
    this.createNurseImageUrl = null;
    this.createPhotoUploadForm.nativeElement.value = '';
  }

  updateTypes(newTypes: {types: Designation[], updatedType: Designation}) {
    if(!newTypes){
      return;
    }
    // Check a Staff to see if they have oldName and update it if they do
    let updateNurseType = (nurse : Staff, oldName: string, updatedType : Designation) => {
      if (nurse.staff_type == oldName){
        nurse.staff_type = updatedType.name;
      }
    }
    // Check and update all Staff which have the old type name
    if (newTypes["updatedType"]){
      let oldName = this.nurseTypes
        .filter(t => t.designation_id == newTypes.updatedType.designation_id)[0]
        .name;
      for(let id in this.nurses){
        updateNurseType(this.nurses[id], oldName, newTypes.updatedType);
      }
      updateNurseType(this.editnurse, oldName, newTypes.updatedType);
      updateNurseType(this.selectednurse, oldName, newTypes.updatedType);
    }
    // Update all type lists
    if (newTypes["types"]){
      this.nurseTypes = newTypes.types;
      this.typeFilter = this.getFilter(this.nurseTypes.map(t => t.name));
    }
  }

  updateAliasEdit(event:any){
    (<HTMLInputElement>document.getElementById('alias-edit')).value = event.target.value;
    this.editnurse.alias = event.target.value;
  }

  updateAliasAdd(event:any){
    (<HTMLInputElement>document.getElementById('alias-add')).value = event.target.value;
    this.newnurse.alias = event.target.value;
  }
}
