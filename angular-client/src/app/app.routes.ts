import { Routes, RouterModule } from '@angular/router';

import { DashboardComponent } from './dashboard/dashboard.component';
import { AllocationHistoryComponent } from './allocationhistory/allocationhistory.component'
import { PodAComponent } from './team/pods/poda/poda.component';
import { PodBComponent } from './team/pods/podb/podb.component';
import { PodCComponent } from './team/pods/podc/podc.component';
import { PodDComponent } from './team/pods/podd/podd.component';
import { SeniorStaffComponent } from './team/seniorstaff/seniorstaff.component';
import { NurseprofileComponent } from './nurseprofile/nurseprofile.component';
import { NurseSidebarComponent } from './nurse-sidebar/nurse-sidebar.component';
import { SettingsComponent } from './settings/settings.component';

const routes: Routes = [
  {
    path: 'sidebar',
    component: NurseSidebarComponent
  },
  {
    path: 'settings',
    component: SettingsComponent
  },
  {
  	path: 'dashboard',
  	component: DashboardComponent
  },
  {
  	path: 'allocationhistory',
  	component: AllocationHistoryComponent
  },
  {
  	path: 'poda',
  	component: PodAComponent
  },
  {
  	path: 'podb',
  	component: PodBComponent
  },
  {
  	path: 'podc',
  	component: PodCComponent
  },
  {
  	path: 'podd',
  	component: PodDComponent
  },
  {
    path: 'seniorstaff',
    component: SeniorStaffComponent
  },
  {
    path: 'nurseprofile',
    component: NurseprofileComponent
  },
  {
    path: '',
    redirectTo: '/dashboard',
    pathMatch: 'full'
  }
];

export const routing = RouterModule.forRoot(routes);
