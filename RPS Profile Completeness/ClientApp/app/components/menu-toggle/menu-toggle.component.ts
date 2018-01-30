import { Component, EventEmitter, Input, Output } from '@angular/core';
import { AppService } from '../../services/app.service';

@Component({
  selector: 'nbs-menu-toggle',
  templateUrl: './menu-toggle.component.html',
  styleUrls: ['./menu-toggle.component.scss']
})
export class MenuToggleComponent {
  @Output() open = new EventEmitter<boolean>();
  @Input() active: boolean = false;

  constructor(private appService: AppService) {
    this.appService.sideNavShown$.subscribe((value) => {
      this.active = value;
    });
  }

  toggle() {
    this.appService.showSideNav(!this.active);
  }

}
