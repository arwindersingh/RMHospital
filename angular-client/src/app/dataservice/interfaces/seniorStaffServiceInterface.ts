import { SeniorStaffAllocation } from '../models/seniorstaffallocation';


export interface SeniorStaffServiceInterface{

	//get current senior allocation by team name
	getSeniorAllocation(teamName: string): Promise<SeniorStaffAllocation>;

	//get senior allocation history by team name and date
	getSeniorAllocationHistory(teamName: string, timestamp: number): Promise<SeniorStaffAllocation>;

	//update current senior allcation bt team name
	updateSeniorAllocation(name: string, senior: SeniorStaffAllocation): Promise<SeniorStaffAllocation>;

}
