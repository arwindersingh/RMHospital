import { Component, OnInit, Output,EventEmitter } from '@angular/core';
import { DesignationService } from '../../dataservice/designation.service';
import { Designation } from '../../dataservice/models/designation';
interface TypeDict {
  [id: number] : {designation: Designation, readonly: boolean};
}

@Component({
  selector: 'typepane',
  templateUrl: './typepane.component.html',
  styleUrls: ['./typepane.component.css'],
  providers: [DesignationService]
})
export class TypePaneComponent implements OnInit {
  @Output() typeChange = new EventEmitter();
  typeDict:TypeDict = {};
 
  constructor(private designationService: DesignationService) { }

  ngOnInit() {
    this.typeDict[-1] = {designation:{name:"",designation_id:-1}  , readonly: true};
    this.designationService.getHttpDesignation().then(designation => {
       designation.forEach(t => this.typeDict[t.designation_id] = {designation:t, readonly:true});
     });
  }

  private save(id: number) {
    this.setReadonly(id);
    if(!this.typeDict[id].designation.name){
      alert("Can't save empty staff type");
      return;
    }
    for(let idx in this.typeDict){
      if (idx == id.toString()){
        // don't check this element against itself
        continue;
      }
      if(this.typeDict[id].designation.name==this.typeDict[idx].designation.name){
        alert("The staff type already exists!");
        return;
      }
    }
    this.designationService.putHttpDesignation({name:this.typeDict[id].designation.name,designation_id:this.typeDict[id].designation.designation_id})
    .then(designation => {
      this.typeDict[id].designation = designation;
      this.typeChange.emit({
         types: this.getDesignationArray(this.typeDict), 
         updatedType: designation
      });
    });
  }

  private delete(id: number) {
    this.designationService.deleteHttpDesignation(id)
    .then(response => {
      if(response["result"]){
        delete this.typeDict[id];
        this.typeChange.emit({
          types: this.getDesignationArray(this.typeDict)
        });
      } else {
        alert("Delete failed. Reason: " + response["reason"]);
      }
    });
  }

  private setReadonly(id: number){
    this.typeDict[id].readonly = true;
    let inputElement = <HTMLInputElement>document.getElementById('type'+id);
    inputElement.readOnly = true;
  }

  edit(id: number){
    this.typeDict[id].readonly = false;
    let inputElement = <HTMLInputElement>document.getElementById('type'+id);
    inputElement.readOnly = false;
    inputElement.focus();
    inputElement.select();
    for (let sid in this.typeDict){
      if (!this.typeDict[id].readonly && sid != id.toString()){
        this.setReadonly(parseInt(sid));
      }
    }
  }

  private newType(){
    this.setReadonly(-1);
    if(!this.typeDict[-1].designation.name){
      alert("Can't save empty staff type");
      return;
    }
    for(let idx in this.typeDict){
      if (idx == "-1"){
        // don't check this element against itself
        continue;
      }
      if(this.typeDict[-1].designation.name==this.typeDict[idx].designation.name){
        alert("The staff type already exists!");
        return;
      }
    }
    if(this.typeDict[-1].designation.name!=""){
      this.designationService.postHttpDesignation(this.typeDict[-1].designation.name)
      .then(Designation => {
          this.typeDict[Designation.designation_id] = {designation: Designation, readonly: true};
          this.typeDict[-1].designation = {name:"", designation_id:-1};
          this.typeChange.emit({
            types: this.getDesignationArray(this.typeDict)
          });
       });
    }
  }

  dictToArray(typeDict):{designation: Designation, readonly: boolean}[] {
    let type:{designation: Designation, readonly: boolean}[] = [];
    for (let id in typeDict){
      if (id == "-1") {
        continue;
      }
      if (typeDict.hasOwnProperty(id)) {
        type.push(typeDict[id]);
      }
    }
    return type;
  }

  private getDesignationArray(typeDict):Designation[] {
    let designation:Designation[] = [];
    let arr:{designation: Designation, readonly: boolean}[] = this.dictToArray(typeDict)
    for(let d of arr){
      designation.push(d.designation);
    }
    return designation;
  }
}