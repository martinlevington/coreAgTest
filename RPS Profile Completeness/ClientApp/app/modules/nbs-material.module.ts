import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatIconRegistry, MatInputModule, MatSelectModule } from '@angular/material';
import { DomSanitizer } from '@angular/platform-browser';


import {
  MatButtonModule, MatCardModule, MatCheckboxModule, MatIconModule, MatSidenavModule,
  MatToolbarModule, MatGridListModule, MatTabsModule, MatMenuModule, MatTooltipModule,
  MatExpansionModule
  } from '@angular/material';

import "hammerjs";

const COMPONENT_LIST = [
  MatButtonModule,
  MatCheckboxModule,
  MatCardModule,
  MatToolbarModule,
  MatSidenavModule,
  MatIconModule,
  MatInputModule,
  MatSelectModule,
  MatGridListModule,
  MatTabsModule,
  MatMenuModule,
  MatTooltipModule,
  MatExpansionModule
];

@NgModule({
  imports: [
    CommonModule,
    ...COMPONENT_LIST
  ],
  exports: COMPONENT_LIST,
  declarations: []
})
export class NbsMaterialModule {
  constructor(iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) {

    // set up our SVG icons
    // iconRegistry.addSvgIcon(
    //   'nbs-logo',
    //   sanitizer.bypassSecurityTrustResourceUrl('assets/icons/nbs-logo.svg'));

    iconRegistry.addSvgIconSetInNamespace("nbs",
      sanitizer.bypassSecurityTrustResourceUrl('assets/icons/symbol-defs.svg'));
    iconRegistry.addSvgIconSetInNamespace("simple",
      sanitizer.bypassSecurityTrustResourceUrl('assets/icons/simple-line-icons.svg'));

  }

}
