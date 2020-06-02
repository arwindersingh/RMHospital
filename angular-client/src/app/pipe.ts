import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
	name: 'searchPipe'
})
export class searchPipe implements PipeTransform {
	transform(value: any[], name: string, type: string, skill:string): any {
		
		//value is the input nurselist
		let output = value;
		//if name is not empty, use name to filter nurse
		if(name != ""){
			//the a nurse contain name, put it into ouput nurse list
			output = value.filter(input => input.first_name.toUpperCase().indexOf(name.toUpperCase()) > -1);
		}
		//if type is not empty, use type to filter nurse
		if(type != ""){
			//the a nurse contain type, put it into ouput nurse list
			output = output.filter(input =>input.staff_type.toUpperCase().indexOf(type.toUpperCase()) > -1);
		}
		//if skill is not empty, use type to filter nurse
		if(skill != ""){
			output = output.filter(input =>{
			//if nurse contain the skills, put it into ouput nurse list
			for(var i= 0; i< input.skills.length;i++){
				if(input.skills[i].toUpperCase().indexOf(skill.toUpperCase()) > -1){
					return true;
				}
			}
			//this nurse does not has this skill
			return false;
		});
		}
		//return the filtered nurselist
		return output;
	}
}

@Pipe({
	name: 'searchprofilePipe'
})

//filter nurses in nurse profile by name
export class searchprofilePipe implements PipeTransform {
	transform(value: any[], args: string): any {
		var output = value.filter(input => input.first_name.toUpperCase().indexOf(args.toUpperCase()) > -1);
		return output;
	}
}