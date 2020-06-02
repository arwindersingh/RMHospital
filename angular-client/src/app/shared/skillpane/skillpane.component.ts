import { Component, OnInit, Output,EventEmitter } from '@angular/core';
import { SkillService } from '../../dataservice/skill.service';
import { Skill } from '../../dataservice/models/skill';

interface SkillDict {
  [id: number] : {skill: Skill, readonly: boolean};
}

@Component({
  selector: 'skillpane',
  templateUrl: './skillpane.component.html',
  styleUrls: ['./skillpane.component.css'],
  providers: [SkillService]
})
export class SkillPaneComponent implements OnInit {
  @Output() skillChange = new EventEmitter();
  skillDict:SkillDict = {};
 
  constructor(private skillService: SkillService) { }

  ngOnInit() {
    this.skillDict[-1] = {skill: {name:"", id:-1}, readonly: true};
    this.skillService.getHttpSkills().then(skills => {
       skills.forEach(s => this.skillDict[s.id] = {skill:s, readonly:true});
     });
  }

  private save(id: number) {
    this.setReadonly(id);
    if(!this.skillDict[id].skill.name){
      alert("Can't save empty skill");
      return;
    }
    for(let idx in this.skillDict){
      if (idx == id.toString()){
        // don't check this element against itself
        continue;
      }
      if(this.skillDict[id].skill.name==this.skillDict[idx].skill.name){
        alert("The skill already exists!");
        return;
      }
    }
    this.skillService.putHttpSkill(this.skillDict[id].skill.name, id)
    .then(skill => {
      this.skillDict[id].skill = skill;
      this.skillChange.emit({
         skills: this.getSkillsArray(this.skillDict), 
         updatedSkill: skill
      });
    });
  }

  private delete(id: number) {
    this.skillService.deleteHttpSkill(id)
    .then(response => {
      if(response["result"]){
        delete this.skillDict[id];
        this.skillChange.emit(
          {skills: this.getSkillsArray(this.skillDict)}
        );
      } else {
        alert("Delete failed. Reason: " + response["reason"]);
      }
    });
  }

  private setReadonly(id: number){
    this.skillDict[id].readonly = true;
    let inputElement = <HTMLInputElement>document.getElementById('input'+id);
    inputElement.readOnly = true;
  }

  edit(id: number){
    this.skillDict[id].readonly = false;
    let inputElement = <HTMLInputElement>document.getElementById('input'+id);
    inputElement.readOnly = false;
    inputElement.focus();
    inputElement.select();
    for (let sid in this.skillDict){
      if (!this.skillDict[id].readonly && sid != id.toString()){
        this.setReadonly(parseInt(sid));
      }
    }
  }

  private newSkill(){
    this.setReadonly(-1);
    if(!this.skillDict[-1].skill.name){
      alert("Can't save empty skill");
      return;
    }
    for(let idx in this.skillDict){
      if (idx == "-1"){
        // don't check this element against itself
        continue;
      }
      if(this.skillDict[-1].skill.name==this.skillDict[idx].skill.name){
        alert("The skill already exists!");
        return;
      }
    }
    this.skillService.postHttpSkill(this.skillDict[-1].skill.name)
    .then(skill => {
        this.skillDict[skill.id] = {skill: skill, readonly: true};
        this.skillDict[-1].skill = {name:"", id:-1};
        this.skillChange.emit({
          skills: this.getSkillsArray(this.skillDict)
        });
      });
  }

  dictToArray(skillDict):{skill: Skill, readonly: boolean}[] {
    let skills:{skill: Skill, readonly: boolean}[] = [];
    for (let id in skillDict){
      if (id == "-1") {
        continue;
      }
      if (skillDict.hasOwnProperty(id)) {
        skills.push(skillDict[id]);
      }
    }
    return skills;
  }

  private getSkillsArray(skillDict):Skill[] {
    let skills:Skill[] = [];
    let arr:{skill: Skill, readonly: boolean}[] = this.dictToArray(skillDict)
    for(let d of arr){
      skills.push(d.skill);
    }
    return skills;
  }
}