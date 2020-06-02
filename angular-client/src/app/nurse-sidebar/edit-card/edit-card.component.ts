import { Component, OnInit,Input,Output,EventEmitter,OnChanges } from '@angular/core';
import { Staff } from '../../dataservice/models/staff';

@Component({
  selector: 'edit-card',
  templateUrl: './edit-card.component.html',
  styleUrls: ['./edit-card.component.css']
})
export class EditCardComponent implements OnInit {
  @Input() staff;
  @Input() position;
  @Input() shift_type;
  @Output() delete = new EventEmitter<string>();
  @Output() noteEmitter = new EventEmitter<{id: string, note: string}>();

  css = {"background-color":"grey"}
  
  constructor() {}

  ngOnInit() {
  }

  changeHeaderColor(){
    if(this.staff.shift_type=="12" && this.staff.staff_name!=""){
      this.css["background-color"] = "maroon";
    }else if (this.staff.shift_type=="8" && this.staff.staff_name!=""){
      this.css["background-color"] = "#337ab7";
    }else{
       this.css["background-color"] = "grey";
    }
  }

  deleteStaff(){
  	this.delete.emit(this.position);
  }

  saveNote(){
    this.noteEmitter.emit({id: this.staff.id, note: this.staff.notes[0]});
  }

  ngOnChanges(...args: any[]) {
    this.changeHeaderColor();
  }
 
}
