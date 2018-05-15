import { Component, EventEmitter, Input, Output, OnInit, OnDestroy, ElementRef, Renderer2, ViewChild } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';

import { ResizeService } from '../../../services/resize-service';
import * as Plotly from 'plotly.js/dist/plotly.js';
import { read } from 'fs';


@Component({
  selector: 'plotlychart',
  templateUrl: './plotly.component.html',
  styleUrls: ['./plotly.component.scss']

})

export class PlotlyComponent implements OnInit, OnDestroy {

  @Input() data: any;
  @Input() layout: any;
  @Input() options: any;
  @Input() displayRawData: boolean = false;

  

  @ViewChild('myPlotlyDiv', { read: ElementRef }) elMyPlotlyDiv: ElementRef;

  private elementWrapperDiv: string = 'myPlotlyDiv';
  private readonly resizeSubscription: Subscription;


  constructor(private resizeService: ResizeService) {
    this.resizeSubscription = this.resizeService.onResize$.subscribe(resize => this.resizeGraph(resize));
  
  }

  resizeGraph(size) {

    Plotly.Plots.resize(this.elMyPlotlyDiv.nativeElement);
    console.log(size);
  }

  ngOnInit() {
    console.log('ngOnInit PlotlyComponent');
    console.log(this.data);
    console.log(this.layout);

    Plotly.newPlot(this.elementWrapperDiv, this.data, this.layout, this.options);
  }


  ngOnDestroy(): void {
    if (this.resizeSubscription) {
      this.resizeSubscription.unsubscribe();
    }
  }
}
