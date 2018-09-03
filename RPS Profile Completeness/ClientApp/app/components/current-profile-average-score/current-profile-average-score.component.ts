import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { TopPerformer } from  '../../models/dashboard/top-performers';



@Component({
    selector: 'current-profile-average-score',
    templateUrl: './current-profile-average-score.component.html',
    styleUrls: ['./current-profile-average-score.component.scss']
})



export class CurrentProfileAverageComponent implements OnInit, OnDestroy {

    currentAverageScore: number;

    ngOnInit(): void {

        this.currentAverageScore = 68;
    }

    ngOnDestroy(): void {
     
    }
}
