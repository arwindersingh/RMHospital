import { Allocation } from './allocation'
import { Staff } from './staff';
import { Beds } from './beds';

export class PodAllocation extends Allocation {
  beds : Staff[];
  consultant :Staff;
  team_leader :Staff;
  registrar :Staff;
  resident :Staff;
  pod_ca :Staff;
  ca_cleaner :Staff;
}