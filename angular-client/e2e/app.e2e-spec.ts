import { AngularClientPage } from './app.po';
import { browser, element, by} from 'protractor'; 
import { Staff } from '../src/app/dataservice/models/staff';

 
 // at the top of the test spec:
 var fs = require('fs');
 //get random names
 function getRandomString(characterLength) {
    var randomText = "";
    var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    for (var i = 0; i < characterLength; i++)
        randomText += possible.charAt(Math.floor(Math.random() * possible.length));
    return randomText;
};
//get random dates
 var randomTimeInWorkday = function(startdate,enddate) {
    var begin = startdate;
    var end = new Date(enddate.getTime());

    begin.setHours(8,0,0,0);
    end.setHours(17,0,0,0);

    return Math.random() * (end.getTime() - begin.getTime()) + begin.getTime();
}
 // abstract writing screen shot to a file
function writeScreenShot(data, filename) {
    var stream = fs.createWriteStream(filename);
    stream.write(new Buffer(data, 'base64'));
    stream.end();
}
//add padding 0 for date and month
function pad(n) {
    return (n < 10) ? ("0" + n) : n;
}
//date format dd/mm/yyyy
function getToday(){
  var today = new Date();
  var dd = pad(today.getDate());
  var mm = pad(today.getMonth()+1); //January is 0!
  var yyyy = today.getFullYear();
  

  return ( dd + "/" + mm + "/" + yyyy);
}

//ranmdom name and shift type
function randomized_staff(staffs){
  for(let s of staffs){
      var name = getRandomString(12);
      s.staff_name = name;
      if(Math.random() < 0.5){
        s.shift_type ="8";
      }
      else{
        s.shift_type="12";  
      }
      s.last_before = randomTimeInWorkday(new Date(2013, 2, 1, 1, 10),new Date()) ;
      //console.log(s.staff_double_before);
      //console.log(new Date(s.staff_double_before));
    };
    return staffs;
}
//generate nurses for any pod 
function pod_nurses(num_nurses,num_staffs,staff_start,staffs){
  for(var i=0;i<staff_start;i++){
      var s = new Staff("","","Nurse","","");
      staffs.push(s);
  }
  for(var i=Number(staff_start);i<num_staffs+staff_start;i++){
      var s = new Staff("","","Staff","","");
      staffs.push(s);
  }
  for(var i=Number(staff_start+num_staffs);i<num_staffs+num_nurses;i++){
      var s = new Staff("","","Nurse","","");
      staffs.push(s);
  }
  return staffs;
}
//validate pod 
function validate_pod(staffs,pod_index){
  var inputNurse;
    var id;
    var shiftType;
    var reflectName;
    var radioButton; 
    for (let s of staffs) {
        reflectName = "shiftTime";
        id = staffs.indexOf(s);
        shiftType = s.shift_type;
        reflectName += pod_index[id];
        inputNurse = element(by.id(pod_index[id]));
        inputNurse.clear();
        inputNurse.sendKeys(s.staff_name);
        if (s.shift_type =="12")
          radioButton = element.all(by.css('[ng-reflect-name='+reflectName+']')).get(1).click();
        else
          radioButton = element.all(by.css('[ng-reflect-name='+reflectName+']')).get(0).click();
    }
    
    let saveSubmitButton = element(by.buttonText('Save'));
    saveSubmitButton.click();
    //authentication
    // var alertDialog = browser.switchTo().alert();
    // alertDialog.sendKeys("acadmin")
    // alertDialog.accept();

    //expect 
    let nurses = element.all(by.css('[type="text"]'));
    // expect(nurses.count()).not.toEqual(22);
    for (let s of staffs) {
        id = staffs.indexOf(s);
        expect(nurses.get(id).getAttribute('value')).toEqual(s.staff_name); 
    }

}

describe('angular-client App', () => {
  let page: AngularClientPage;
  let staffs_a: Array<Staff> = [];
  let staffs_b: Array<Staff> = [];
 
  beforeEach(() => {
    page = new AngularClientPage();

});
  
  it('pod a add nurses automatically', () => {
    staffs_a = pod_nurses(12,6,8,staffs_a);
    staffs_a = randomized_staff(staffs_a);
    
    var pod_a_nurse_index =["A1","A2","A3","A4","A12","A11","A10","A9","L1","L2","L3","L4","L5","L6","A5","A6","A7","A8"];
    
    page.navigateTo();
    browser.get("/poda");
    
    validate_pod(staffs_a,pod_a_nurse_index);

    browser.takeScreenshot().then(function (png) {
        writeScreenShot(png, './e2e/poda.png');
    });
    
  });
  it('pod b add nurses automatically', () => {
    browser.get("/podb");

    staffs_b = pod_nurses(10,6,4,staffs_b);
    staffs_b = randomized_staff(staffs_b);
    
    var pod_b_nurse_index =["B10","B9","B8","L1","L2","L3","L4","L5","L6","B1","B2","B3","B7","B6","B5","B4"];

    validate_pod(staffs_b,pod_b_nurse_index);

   

    browser.takeScreenshot().then(function (png) {
        writeScreenShot(png, './e2e/podb.png');
    });
    
  });
  
  it('dashboard updates nurses accordingly', () => {
    browser.get("/dashboard");
    var id = 0;
    var inputNurses_a = element.all(by.css('.poda dashcell [name]'));
    var inputNurses_b = element.all(by.css('.podb dashcell [name]'));

   for (let s of staffs_a) {
        id = staffs_a.indexOf(s);
        expect(inputNurses_a.get(id).getText()).toEqual(s.staff_name);   
    }
    for (let s of staffs_b) {
        id = staffs_b.indexOf(s);
        expect(inputNurses_b.get(id).getText()).toEqual(s.staff_name);   
    }
    
    browser.takeScreenshot().then(function (png) {
        writeScreenShot(png, './e2e/dashboard.png');
    });

  });
  it('can check the history of the nurses', () => {
    var id;

    browser.get("/allocationhistory");

    var datepicker = element(by.css('.selectiongroup input'));
    var timepicker = element(by.css('select option[value="23"]')).click(); 
    var date = getToday();
    var unixtime = Date.now();

    datepicker.clear();
    datepicker.sendKeys(date);
    var search = element(by.buttonText("Search")).click();
    browser.takeScreenshot().then(function (png) {
        writeScreenShot(png, './e2e/allocation.png');
    });
    //browser.refresh();
    var inputNurses_a = element.all(by.css('.poda dashcell [name]'));
    var inputNurses_b = element.all(by.css('.podb dashcell [name]'));
    for(let s of staffs_a){

        id = staffs_a.indexOf(s);
        if(Number(s.last_double)<=unixtime){
           expect(inputNurses_a.get(id).getText()).toEqual(s.staff_name);
        }
        else{
           expect(inputNurses_a.get(id).getText()).toEqual('');
        }
        
    }
    for(let s of staffs_b){

        id = staffs_b.indexOf(s);
        if(Number(s.last_double)<=unixtime){
           expect(inputNurses_b.get(id).getText()).toEqual(s.staff_name);
        }
        else{
           expect(inputNurses_b.get(id).getText()).toEqual('');
        }
        
    }
    //browser.sleep(10000);
    
  });
});
