import { Component, Input } from '@angular/core';

@Component({
  selector: 'dashcell',
  templateUrl: './dashcell.component.html',
  styleUrls: ['./dashcell.component.css']
})

export class DashCellComponent {
  @Input() name;
  @Input() title;
  @Input() id;
  @Input() shifttype;
  @Input() note;

  // Format Titles 
  formatString(str : string) {
    var formatTitles = {
      "consultant" : "Consultant",
      "team_leader" : "Team Leader",
      "registrar" : "Registrar",
      "resident" : "Resident",
      "pod_ca" : "POD CA",
      "ca_cleaner" : "CA Cleaner"
    };
    if(str != "" && str != null){
      if (str in formatTitles)
        return formatTitles[str]
    
    str = str.charAt(0).toUpperCase() + str.slice(1);
    return str.replace(/_/g, ' ');
    }else{
      return "";
    }
  }

  // Return full first name and initial of last name
  firstNameLastInitial(str : string) {
    if (!this.id){
      // if the staff doesn't have an id, don't change the string
      return str;
    }
    if(str){
      var names = str.split(" ");
      var fNameLInitial = names[0] + " ";

      if(names.length > 1) {
        fNameLInitial += names[names.length - 1].substring(0, 1).toUpperCase();
      }
      return fNameLInitial;
    }else{
      return "";
    }
  }

}
