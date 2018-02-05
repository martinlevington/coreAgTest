import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'nbs-nav-panel',
  templateUrl: "nbs-nav-panel.component.html",
  styleUrls: ['nbs-nav-panel.component.scss']
})
export class NbsNavPanelComponent implements OnInit {

  menuItems = [
    {icon: "simple:home", title: "Home", children: null, routerLink: ['/', 'home']},
    { icon: "simple:vector", title: "Plotly", children: null, routerLink: ['/', 'plotly'] },
    { icon: "simple:vector", title: "Snake Region", children: null, routerLink: ['/', 'snake-region'] },

    { icon: "simple:printer", title: "Space", children: null, routerLink: ['/', '#'] },
    {icon: "simple:screen-desktop", title: "Brand for Digital", children: null, routerLink: ['/', 'home']},
    {icon: "simple:docs", title: "Development Framework", children: null, routerLink: ['/', 'dev-framework']},
    {icon: "simple:cloud-download", title: "Nimbus Style Guide", children: [
      {title: "Typography", routerLink: ['/nimbus', 'typography']},
      {title: "Breadcrumb", routerLink: ['/nimbus', 'breadcrumb']},
      {title: "Buttons and Links", routerLink: ['/nimbus', 'buttons']},
      {title: "Footer", routerLink: ['/nimbus', 'footer']},
      {title: "Forms", routerLink: ['/nimbus', 'forms']},
      {title: "Toolbar", routerLink: ['/nimbus', 'toolbar']},
      {title: "Project Cards", routerLink: ['/nimbus', 'project-card']},
    ]},
  ];

  constructor() { }

  ngOnInit() {
  }

  showChildren(event) {
    event.target.siblings.classList.remove('opened');
    event.target.classList.add('opened');
  }
}
