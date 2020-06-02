import { Component, OnInit } from '@angular/core';
import { SkillPaneComponent } from '../shared/skillpane/skillpane.component';
import { TypePaneComponent } from '../shared/typepane/typepane.component';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css'],
  providers: []
})
export class SettingsComponent implements OnInit {
 
  constructor() { }

  ngOnInit() {} 
}