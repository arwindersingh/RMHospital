import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { PodAComponent } from './poda.component';
import { HttpModule } from '@angular/http';

describe('PodAComponent', () => {
  let component: PodAComponent;
  let fixture: ComponentFixture<PodAComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ FormsModule, HttpModule],
      declarations: [ PodAComponent]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PodAComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
describe ('manually add nurses' , function(){
    beforeEach(function() {
      
    });
    it('should should add nurses');
});