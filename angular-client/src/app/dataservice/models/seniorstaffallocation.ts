import { Allocation } from './allocation'
import { Staff } from './staff';
export class SeniorStaffAllocation extends Allocation {
  access_coordinator : Staff;
  tech : Staff;
  mern : Staff;
  ca_support : Staff;
  ward_consultant : Staff;
  transport : Staff;
  donation : Staff;
  cnm : Staff[];
  cnc : Staff[];
  resource : Staff[];
  internal : Staff[];
  external : Staff[];
  educator : Staff[];
}