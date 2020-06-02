import { Component } from '@angular/core';
import { ValueService } from './value.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [ValueService]
})
export class AppComponent {
  title = 'Allogator';
}
