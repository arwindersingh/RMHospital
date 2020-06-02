import { PodAllocation } from '../models/podallocation';

export interface PodServiceInterface{

	//get current allocation by team name
	getPodAllocation(teamName: string): Promise<PodAllocation>;

	//get allocation history by team name and date
	getPodAllocationHistory(teamName: string, timestamp: number): Promise<PodAllocation>;

	//update current pod information by team name
	updatePodAllocation(teamName: string, pod: PodAllocation): Promise<PodAllocation>;

}