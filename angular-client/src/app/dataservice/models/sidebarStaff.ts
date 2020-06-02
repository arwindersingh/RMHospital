import { Staff } from "./staff";

export class sidebarStaff extends Staff{
    tasks: string[];

     constructor(staff:Staff, beds:string[]=[])
     {
        super(staff.first_name, staff.shift_type, staff.last_name,staff.alias,staff.staff_type,staff.id,staff.photo,staff.skills,staff.last_double,staff.rosteron_id);
        this.tasks = beds;
    }
}
