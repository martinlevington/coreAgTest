import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MonthlyAveragePerformer } from  '../../models/dashboard/monthly-average-performer';



@Component({
    selector: 'monthly-profile-average-score',
    templateUrl: './monthly-profile-average-score.html',
    styleUrls: ['./monthly-profile-average-score.scss']
})



export class MonthlyAveragePerformersComponent implements OnInit, OnDestroy {

    performersData : Array<MonthlyAveragePerformer> ;

    public plotlyLayout: any;
    public plotlyData: any;
    public plotlyOptions: any;

    ngOnInit(): void {

        this.performersData = [
            { Month: "January", Score : 60.2 },
            { Month: "February", Score: 62.0 },
            { Month: "March", Score: 64.0 },
            { Month: "April", Score: 65.0 },
            { Month: "May", Score: 67.0 },
            { Month: "June", Score: 69.0 }
        ];

        this.plotlyLayout = {
            title: "Monthly Avg",
            height: 500,
            width: 1200
          };

          this.plotlyData = [
            {
              x: this.performersData,
              y: this.performersData,
              name: "Avg",
              type: 'bar',
              orientation: 'v'
            }
          ];
    }

    ngOnDestroy(): void {
     
    }
}
