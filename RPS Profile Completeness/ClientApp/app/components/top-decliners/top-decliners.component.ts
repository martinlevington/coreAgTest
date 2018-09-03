import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { TopPerformer } from  '../../models/dashboard/top-performers';




@Component({
    selector: 'top-decliners',
    templateUrl: './top-decliners.component.html',
    styleUrls: ['./top-decliners.component.scss']
})



export class TopDeclinersComponent implements OnInit, OnDestroy {

    performers : Array<TopPerformer> ;

    ngOnInit(): void {

        this.performers = [
            { Name: "One", Change: 1, Score : 78.2 },
            { Name: "Two", Change : 2, Score: 60.0 }
        ];

        this.performers.sort(function(a: TopPerformer, b: TopPerformer) {return a.Change<b.Change ? a.Change : b.Change;})
    }

    ngOnDestroy(): void {

    }
}
