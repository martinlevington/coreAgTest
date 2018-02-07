import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { TopPerformer } from  '../../models/dashboard/top-performers';



@Component({
    selector: 'top-performers',
    templateUrl: './top-performers.component.html',
    styleUrls: ['./top-performers.component.scss']
})



export class TopPerformersComponent implements OnInit, OnDestroy {

    topPerformers : Array<TopPerformer> ;

    ngOnInit(): void {

        this.topPerformers = [
            { Name: "One", Change: 1, Score : 78.2 },
            { Name: "Two", Change : 2, Score: 60.0 }
        ];
    }

    ngOnDestroy(): void {
         throw new Error("Not implemented");
    }
}
