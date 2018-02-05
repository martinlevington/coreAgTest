import { Component, Input } from '@angular/core';
import { Router } from '@angular/router';
import { AppService } from '../../../services/app.service';

interface NavMenuItem {
  icon: string,
  title: string,
  children?: any,
  routerLink?: any,
}

@Component({
  selector: 'nbs-side-nav-menu-item',
  templateUrl: './nav-menu-item.component.html',
  styleUrls: ['./nav-menu-item.component.scss'],
})
export class NavMenuItemComponent {
  @Input() public menuItem: NavMenuItem;

  constructor(
    private router: Router,
    private appService: AppService,
  ) { }

  public linkClicked(target: NavMenuItem) {
    // if this menu item does not have a link to navigate to, we don't need to do anything
    if (!target || !target.routerLink) {
      return;
    }

    this.router.navigate(target.routerLink);
    // collapse the side nav after we navigate to a different route from the side nav
    this.appService.showSideNav(false);
  }
}
