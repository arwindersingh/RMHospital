import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { PodCComponent } from './podc.component';
import { HttpModule } from '@angular/http';
//import { PodService } from '../../dataservice/pod.service';

describe('PodCComponent', () => {
  let component: PodCComponent;
  let fixture: ComponentFixture<PodCComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ FormsModule, HttpModule],
      declarations: [ PodCComponent ],
 //     providers: [PodService]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PodCComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
