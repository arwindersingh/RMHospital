import { Staff } from '../models/staff';


export interface StaffServiceInterface{

	//get all staffs in database
	getAllStaff(): Promise<Staff>;

	//get one particular staff by staff id
	getStaffById(id: string): Promise<Staff>;

	//get a list of staffs by conditions
	getStaffByCondition(conditions: string[]): Promise<Staff[]>;

	//create a new staff
	createNewStaff(newStaff: Staff): Promise<Staff>;

	//delete a staff in database by staff id
	deleteStaff(deleteStaffId: string): Promise<Staff>;

	//update a staff information 
	updateStaff(updateStaff: Staff): Promise<Staff>;

	//get all skills of staff  in database
	getAllSkills(): Promise<string[]>;

}