import { TestBed, async } from '@angular/core/testing';

import { AppComponent } from './app.component';
import { NavigationComponent } from './navigation/navigation.component';
import { routing } from './app.routes';


describe('AppComponent', () => {
  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [
        AppComponent,
        NavigationComponent,
        routing
      ],
    }).compileComponents();
  }));
  })

