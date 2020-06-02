import { Component, Input, OnInit,forwardRef,Host,Inject,OnChanges,EventEmitter,Output  } from '@angular/core';
import { NurseSidebarComponent } from '../nurse-sidebar.component';
import { Staff } from '../../dataservice/models/staff'

@Component({
  selector: 'sidebar-cell',
  templateUrl: './share.component.html',
  styleUrls: ['./share.component.css']
})
export class ShareComponent implements OnInit{

  @Input() team;
  @Input() bednum;
  @Input() staff:Staff;
  @Input() shift_type;
  @Output() clickCell = new EventEmitter<Staff>();

  closed = false;
  css = {"background-color":"white"}
  
  constructor(){}

  ngOnInit(){
  	this.renderColor();
  }
  //change color based on shift_type
  ngOnChanges(...args: any[]) {
       this.renderColor();
    }

   renderColor(){
    if(this.staff.shift_type == "12"){
      this.css["background-color"] = "maroon";
    }else if(this.staff.shift_type == "8"){
      this.css["background-color"] = "#337ab7";
    }else{
      this.css["background-color"] = "grey";
    }

    if(this.shift_type=="closed"){
      this.closed = true;
    }else{
      this.closed = false;
    }
   }

   getTitle(){
     if(this.team){
       return this.team + this.bednum;
     }
     else{
      var formatTitles = {
        "consultant" : "Consultant",
        "team_leader" : "Team Leader",
        "registrar" : "Registrar",
        "resident" : "Resident",
        "pod_ca" : "POD CA",
        "ca_cleaner" : "CA Cleaner"
      };
      var str = this.bednum;
      if(str != "" && str != null){
        if (str in formatTitles)
          return formatTitles[str]
      str = str.charAt(0).toUpperCase() + str.slice(1);
      return str.replace(/_/g, ' ');
      }else{
        return "";
      }
     }
   }
   //select a bed
   clickPanelBody(){
    this.clickCell.emit(this.staff);
  }
}
