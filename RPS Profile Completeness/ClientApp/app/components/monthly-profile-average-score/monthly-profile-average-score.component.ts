import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MonthlyAveragePerformer } from  '../../models/dashboard/monthly-average-performer';

import { PlotlyComponent } from '../../shared/components/plotly/plotly.component';
import { Subscription } from 'rxjs/Subscription';
import { ResizeService } from '../../services/resize-service';


@Component({
    selector: 'monthly-profile-average-score',
    templateUrl: './monthly-profile-average-score.component.html',
    styleUrls: ['./monthly-profile-average-score.component.scss']
})



export class MonthlyAveragePerformersComponent implements OnInit, OnDestroy {

    performersData: Array<MonthlyAveragePerformer> ;
    private resizeSubscription: Subscription;
    
    public plotlyLayout: any;
    public plotlyData: any;
    public plotlyOptions: any;

    constructor(private resizeService: ResizeService) {
        this.resizeSubscription = this.resizeService.onResize$
        .subscribe(size => console.log(size));
    }

 

 

    ngOnInit(): void {

        this.performersData = [
            { Month: 'January', Score : 60.2 },
            { Month: 'February', Score: 62.0 },
            { Month: 'March', Score: 64.0 },
            { Month: 'April', Score: 65.0 },
            { Month: 'May', Score: 67.0 },
            { Month: 'June', Score: 69.0 }
        ];

        this.plotlyLayout = {
            title: 'Monthly Avg',
            font: {
                family: 'Roboto, "Helvetica Neue", sans-serif',
                fontSize: '24px',
                fontWeight: '400',
                color: '#592d5e'
            },
            autosize: true,
            showlegend: false
          };

          this.plotlyData = [
            {
              x: this.getXData(),
              y: this.getYData(),
              name: 'Avg',
              type: 'bar',
              orientation: 'v'
            }
          ];

          this.plotlyOptions = { 
              displayModeBar: false,
              staticPlot: true
             };
    }


    getXData() {
        let x = [];
        for (let i of this.performersData) {
            x.push(i.Month);
        }

        return x;
    }

    getYData() {
        let x = [];
        for (let i of this.performersData) {
            x.push(i.Score);
        }

        return x;
    }

    ngOnDestroy(): void {

    }
}
