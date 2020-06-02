import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { PodBComponent } from './podb.component';
import { HttpModule } from '@angular/http';
//import { PodService } from '../../dataservice/pod.service';

describe('PodBComponent', () => {
  let component: PodBComponent;
  let fixture: ComponentFixture<PodBComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ FormsModule, HttpModule],
      declarations: [ PodBComponent ],
 //     providers: [PodService]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PodBComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
