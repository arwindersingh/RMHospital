import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';

import { PodDComponent } from './podd.component';
import { HttpModule } from '@angular/http';
//import { PodService } from '../../dataservice/pod.service';

describe('PodDComponent', () => {
  let component: PodDComponent;
  let fixture: ComponentFixture<PodDComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ FormsModule, HttpModule],
      declarations: [ PodDComponent ],
 //     providers: [PodService]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PodDComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
