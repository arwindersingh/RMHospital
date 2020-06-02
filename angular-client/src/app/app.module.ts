import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { routing } from './app.routes';
import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { UploadCsvComponent } from './nurse-sidebar/uploadcsv/uploadcsv.component';

import { PodComponent } from './team/pods/pod.component';
import { TeamComponent } from './team/team.component';
import { PodAComponent } from './team/pods/poda/poda.component';
import { PodBComponent } from './team/pods/podb/podb.component';
import { PodCComponent } from './team/pods/podc/podc.component';
import { PodDComponent } from './team/pods/podd/podd.component';
import { SeniorStaffComponent } from './team/seniorstaff/seniorstaff.component';
import { CellComponent } from './team/shared/cell.component';

import { DashboardComponent } from './dashboard/dashboard.component';
import { AllocationHistoryComponent } from './allocationhistory/allocationhistory.component';
import { DashCellComponent } from './shared/dashcell/dashcell.component';

import { NurseprofileComponent } from './nurseprofile/nurseprofile.component';
import { AccordionModule } from 'ngx-bootstrap/accordion';
import { ModalModule } from 'ngx-bootstrap';
import { PopoverModule } from 'ngx-bootstrap/popover';

import { NurseSidebarComponent } from './nurse-sidebar/nurse-sidebar.component';
import { ShareComponent } from './nurse-sidebar/share/share.component';
import { EditCardComponent } from './nurse-sidebar/edit-card/edit-card.component';
import { MyDatePickerModule } from 'mydatepicker';
import { searchPipe, searchprofilePipe } from './pipe';

import { PapaParseModule } from 'ngx-papaparse';
import { SettingsComponent } from './settings/settings.component';
import { SkillPaneComponent } from './shared/skillpane/skillpane.component';
import { TypePaneComponent } from './shared/typepane/typepane.component';

import { PodService } from './dataservice/pod.service';
import { SideBarService } from './dataservice/sidebar.service';
import { SeniorStaffService } from './dataservice/seniorstaff.service';
import { NurseProfileService } from './dataservice/nurseprofile.service';
import { SkillService } from './dataservice/skill.service';
import { NoteService } from './dataservice/nursenotes.service';
import { ImageService } from './dataservice/image.service';

import { DesignationService } from './dataservice/designation.service';

import { DragulaModule } from 'ng2-dragula';


@NgModule({
  declarations: [
    AppComponent,
    NavigationComponent,
    DashboardComponent,
    DashCellComponent,
    AllocationHistoryComponent,
    TeamComponent,
    PodComponent,
    PodAComponent,
    PodBComponent,
    PodCComponent,
    PodDComponent,
    SeniorStaffComponent,
    NurseprofileComponent,
    CellComponent,
    NurseSidebarComponent,
    ShareComponent,
    EditCardComponent,
    searchPipe,
    searchprofilePipe,
    UploadCsvComponent,
    SettingsComponent,
    SkillPaneComponent,
    TypePaneComponent,
  ],
  imports: [
    DragulaModule,
    BrowserModule,
    FormsModule,
    HttpModule,
    routing,
    MyDatePickerModule,
    AccordionModule.forRoot(),
    ModalModule.forRoot(),
    PopoverModule.forRoot(),
    PapaParseModule,
  ],
  providers: [ 
    PodService,
    SeniorStaffService,
    NurseProfileService,
    SideBarService,
    SkillService,
    ImageService,
    DesignationService,
    NoteService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }