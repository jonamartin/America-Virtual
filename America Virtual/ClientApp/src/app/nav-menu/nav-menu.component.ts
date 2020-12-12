import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  public formVisible: boolean = false;

  toggleForm() {
    this.formVisible = this.formVisible ? false : true;
  }

}