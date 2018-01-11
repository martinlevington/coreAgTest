import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

@Injectable()
export class AppService {

  // sources
  public sideNavShownSource = new BehaviorSubject<boolean>(false);

  // streams
  public sideNavShown$ = this.sideNavShownSource.asObservable();

  public showSideNav(value: boolean) {
    this.sideNavShownSource.next(value);
  }
}
