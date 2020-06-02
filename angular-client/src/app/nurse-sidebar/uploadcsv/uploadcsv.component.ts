import { Component, Output, EventEmitter } from '@angular/core';
import { PapaParseService } from 'ngx-papaparse';

@Component({
  selector: 'uploadcsv',
  templateUrl: './uploadcsv.component.html',
  styleUrls: ['./uploadcsv.component.css'],
})

export class UploadCsvComponent {
  @Output() uploaded = new EventEmitter<string[]>();

  showForm : Boolean;
  private fReader : FileReader;
  private p : PapaParseService;
  startTimes : Set<string>;
  private csvFile : File;
  showFilters : Boolean;
  private selectedTimes : Array<string>;

  constructor(){
    this.showForm = false;
    this.showFilters = false;
    this.fReader = new FileReader();
    this.startTimes = new Set<string>();
    this.fReader.onload = e  => {this.getListOfStartTimes(e)}
  }

  showUploadForm(){
    this.showForm = !this.showForm;
  }

  // Handles uploading and parsing csv
  uploadCSV(e){
    let file = e.target.files[0];
    if(file){
      this.csvFile = file;
      this.fReader.readAsText(file);
    }
  }

  filterByTime(){
    var times = <NodeListOf<HTMLInputElement>> document.getElementsByName("shiftStartTimes");
    var index;
    this.selectedTimes = new Array<string>();
    for(index = 0; index<times.length; index++){
      if(times[index].checked === true)
        this.selectedTimes.push(times[index].value);
    }
    this.fReader.onload = e => {this.parseUploadedCSV(e)};
    this.fReader.readAsText(this.csvFile);
  }
  private getListOfStartTimes(e){
    let rows = this.getRows(e);
    if (!rows) return;
    rows.data.forEach(row => {
      if(this.getCSVField(row, 'blank_line_flg') == 'Y') return;
      let startTime = this.getCSVField(row, 'start_time');
      if(!startTime || startTime.match(/^ *$/) !== null){
        console.log("Error: CSV start time is undefined");
      }
      else
        this.startTimes.add(startTime); 
    });
    this.showFilters = true;
  }
  //attempts to get rows from file object
  private getRows(e){
    let fileContent = e.target.result;
    let p = new PapaParseService();
    let rows = p.parse(fileContent, {header:true});
    // ensure we've passed headers correctly
    if (rows.meta.fields[0] == 'area_id') {
      return rows;
    } else {
      alert('Headers not detected in CSV. Ensure exporting from RosterOn by "CSV (with headers)"');
      return undefined;
    }
  }
  private parseUploadedCSV(e){
    let rows = this.getRows(e);
    if (!rows) return;
    var rosterOnIds : string[] = [];
    rows.data.forEach(row => {
      if(this.getCSVField(row, 'blank_line_flg') == 'Y') return;
      if(this.selectedTimes.indexOf(this.getCSVField(row,'start_time')) >= 0)
        rosterOnIds.push(this.getCSVField(row,'emp_no')); 
    });
    this.showForm = false;
    // Emit the rostered nurses to nurse sidebar
    this.uploaded.emit(rosterOnIds);
  }
  // Attempt to get field name from row
  // CSV may be malformated, checks formatting
  // WARNING: Assumptions: if CSV is malformed, only correctly returns
  // for field = 'emp_no', 'blank_line_flg', 'start_time'
  private getCSVField(row, field){
    if(!row || !field) return undefined; 
    if(!isNaN(row['emp_no'])){
      return row[field];  
    } else {
      // we're dealing with malformed CSV
      let conversionName = {
        'emp_no' : 'emp_id',
        'blank_line_flg' : 'area_page_break_id',
        'start_time' : 'vacancy_flg'
      }
      //Check if we know where to look for this field
      if(!conversionName[field]) return undefined; 
      return row[conversionName[field]];  
    }
  }
}