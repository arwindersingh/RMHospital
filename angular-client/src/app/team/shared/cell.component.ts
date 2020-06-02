import { Component, Input } from '@angular/core';
import { TeamComponent } from '../team.component';
import { ImageService } from '../../dataservice/image.service';


@Component({
  selector: 'cell',
  templateUrl: './cell.component.html',
  styleUrls: ['./cell.component.css']
})

export class CellComponent {
  @Input() alloc;
  @Input() edited;
  @Input() team;
  @Input() staff;
  @Input() bednum;

  css = {"background-color":"white"}

  constructor(
    private imageService: ImageService
  ) {} 

  ngOnInit(){
    this.renderColor();
  }

  ngOnChanges(...args: any[]) {
    this.renderColor();
  }

  // Replace '_' with ' ' and start with a capital letter
  formatString(str : string) {
    str = str.charAt(0).toUpperCase() + str.slice(1);
    return str.replace(/_/g, ' ');
  }
  
  // change the  input color according to the checked radio
  shiftColor(id : string, shiftType : string){
    let button = document.getElementById(id) as HTMLButtonElement;

    if (button != null) {
      switch (shiftType) {
        case '8':
          button.style.backgroundColor = 'lightblue';
          return;

        case '12':
          button.style.backgroundColor = 'mistyrose';
          return;

        case 'closed':
          button.style.backgroundColor = 'LightGray'
          return;

        default:
          button.style.backgroundColor = 'lightblue';
          return;
      }
    } 
    alert("The " + id + " does not exist.");     
  }

  renderColor(){
    // if (this.staff != null && this.alloc[]) {
    //   console.log("^_^:" + this.bednum);
    // }
    //[this.bednum-1].staff_name
    //console.log("**:" + this.alloc["beds"][this.bednum-1].id);
    
    // if(this.staff == null && this.alloc["beds"][this.bednum-1]["staff_name"] != ""){
    //   if(this.alloc["beds"][this.bednum-1].shift_type == "12"){
    //     this.css["background-color"] = "maroon";
    //   }else if(this.alloc["beds"][this.bednum-1].shift_type == "8"){
    //     this.css["background-color"] = "#337ab7";
    //   }else{
    //     this.css["background-color"] = "grey";
    //   }
    // }




    if((this.staff == null && this.alloc["beds"][this.bednum-1].id != null)
      || (this.staff == null && this.alloc["beds"][this.bednum-1]["staff_name"] != "")){
      console.log("**:" + this.alloc["beds"][this.bednum-1].shift_type);
      if(this.alloc["beds"][this.bednum-1].shift_type == "12"){
        this.css["background-color"] = "maroon";
      }else if(this.alloc["beds"][this.bednum-1].shift_type == "8"){
        this.css["background-color"] = "#337ab7";
      }else{
        this.css["background-color"] = "grey";
      }
    }
    if((this.staff != null && this.alloc[this.staff.name].id!= null)
      || (this.staff != null && this.alloc[this.staff.name]["staff_name"] != "")){
      console.log(this.staff.name + "^_^:  "+this.alloc[this.staff.name].shift_type + "-" + this.alloc[this.staff.name].id);

      if(this.alloc[this.staff.name].shift_type == "12"){
        this.css["background-color"] = "maroon";
      }else if(this.alloc[this.staff.name].shift_type == "8"){
        this.css["background-color"] = "#337ab7";
      }else{
        this.css["background-color"] = "grey";
      }
    }
  }

  showInfo(name: string){
    console.log("Allocation: " + name);
  }
}