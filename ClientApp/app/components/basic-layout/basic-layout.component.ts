import { Component, OnInit } from '@angular/core';
import { AppService } from './../../services/app.service';

@Component({
  selector: 'nbs-basic-layout',
  templateUrl: 'basic-layout.component.html',
  styleUrls: ['basic-layout.component.scss'],
})
export class BasicLayoutComponent implements OnInit {
  public showMenu: boolean = false;

  constructor(private appService: AppService) {
    this.appService.sideNavShown$.subscribe((value) => {
      this.showMenu = value;
    });
  }

  ngOnInit() {

  }

  public onNavClose() {
    this.appService.showSideNav(false);
  }

  public onNavOpen() {
    this.appService.showSideNav(true);
  }

}
