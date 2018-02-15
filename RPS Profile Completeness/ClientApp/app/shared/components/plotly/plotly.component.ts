import { Component, EventEmitter, Input, Output, OnInit, OnDestroy, ElementRef } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';

import { ResizeService } from '../../../services/resize-service';
import * as Plotly from 'plotly.js/lib/core';


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


  private resizeSubscription: Subscription;
  private gd;

  constructor(private resizeService: ResizeService) {
    this.resizeSubscription = this.resizeService.onResize$.subscribe(size => console.log(size));
  
  }

  resizeGraph(size)
  {
      Plotly.Plots.resize(this.gd);
  }

  ngOnInit() {
    console.log("ngOnInit PlotlyComponent");
    console.log(this.data);
    console.log(this.layout);

    var d3 = Plotly.d3;
    this.gd = d3.select('myPlotlyDiv');



    Plotly.newPlot('myPlotlyDiv', this.data, this.layout, this.options);
  }


 

  ngOnDestroy(): void {
    if (this.resizeSubscription) {
      this.resizeSubscription.unsubscribe();
    }
  }
}
