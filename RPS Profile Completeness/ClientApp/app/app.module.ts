import { NgModule, Inject } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { CommonModule, APP_BASE_HREF } from '@angular/common';
import { HttpModule, Http, JsonpModule } from '@angular/http';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BrowserModule, BrowserTransferStateModule } from '@angular/platform-browser';
import { TransferHttpCacheModule } from '@nguniversal/common';
import { Configuration } from './app.constants';

import { AppService } from './services/app.service';
import { ResizeService } from './services/resize-service';

// i18n support
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { AppComponent } from './app.component';
import { NbsMaterialModule } from './modules/nbs-material.module';
import { BasicLayoutComponent } from './components/basic-layout/basic-layout.component';
import { MenuToggleComponent } from './components/menu-toggle/menu-toggle.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './containers/home/home.component';
import { UsersComponent } from './containers/users/users.component';
import { UserDetailComponent } from './components/user-detail/user-detail.component';
import { CounterComponent } from './containers/counter/counter.component';
import { NotFoundComponent } from './containers/not-found/not-found.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NbsNavPanelComponent } from './components/nbs-nav-panel/nbs-nav-panel.component';
import { NavMenuItemComponent } from './components/nbs-nav-panel/nav-menu-item/nav-menu-item.component';

import { PlotlyComponent } from './shared/components/plotly/plotly.component';
import { RegionComponent } from './components/snake/region.component';
import { TopPerformersComponent } from './components/top-performers/top-performers.component';
import { TopDeclinersComponent } from './components/top-decliners/top-decliners.component';
import { CurrentProfileAverageComponent } from './components/current-profile-average-score/current-profile-average-score.component';
import { MonthlyAveragePerformersComponent } from './components/monthly-profile-average-score/monthly-profile-average-score.component';



import { LinkService } from './shared/link.service';
import { UserService } from './shared/user.service';
import { ORIGIN_URL } from '@nguniversal/aspnetcore-engine';


export function createTranslateLoader(http: HttpClient, baseHref) {
    // Temporary Azure hack
    if (baseHref === null && typeof window !== 'undefined') {
        baseHref = window.location.origin;
    }
    // i18n files are in `wwwroot/assets/`
    return new TranslateHttpLoader(http, `${baseHref}/assets/i18n/`, '.json');
}

@NgModule({
    declarations: [
      AppComponent,
      BasicLayoutComponent,
      MenuToggleComponent,
      NavMenuComponent,
      CounterComponent,
      UsersComponent,
      UserDetailComponent,
      HomeComponent,
      NbsNavPanelComponent,
      NavMenuItemComponent,
      NotFoundComponent,
      PlotlyComponent,
        RegionComponent,
        TopPerformersComponent,
        TopDeclinersComponent,
        CurrentProfileAverageComponent,
        MonthlyAveragePerformersComponent
    ],
    imports: [
        CommonModule,
        BrowserModule.withServerTransition({
          appId: 'my-app-id' // make sure this matches with your Server NgModule
        }),
        HttpClientModule,
        TransferHttpCacheModule,
        BrowserTransferStateModule,
        BrowserAnimationsModule,

        FormsModule,
        HttpModule,
        JsonpModule,
     
        // i18n support
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: (createTranslateLoader),
                deps: [HttpClient, [ORIGIN_URL]]
            }
        }),
        NbsMaterialModule,
        AppRoutingModule
    

        //// App Routing
        //RouterModule.forRoot([
        //    {
        //        path: '',
        //        redirectTo: 'home',
        //        pathMatch: 'full'
        //    },
        //    {
        //        path: 'home', component: HomeComponent,

        //        // *** SEO Magic ***
        //        // We're using "data" in our Routes to pass in our <title> <meta> <link> tag information
        //        // Note: This is only happening for ROOT level Routes, you'd have to add some additional logic if you wanted this for Child level routing
        //        // When you change Routes it will automatically append these to your document for you on the Server-side
        //        //  - check out app.component.ts to see how it's doing this
        //        data: {
        //            title: 'Homepage',
        //            meta: [{ name: 'description', content: 'This is an example Description Meta tag!' }],
        //            links: [
        //                { rel: 'canonical', href: 'http://blogs.example.com/blah/nice' },
        //                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/' }
        //            ]
        //        }
        //    },
        //    {
        //        path: 'counter', component: CounterComponent,
        //        data: {
        //            title: 'Counter',
        //            meta: [{ name: 'description', content: 'This is an Counter page Description!' }],
        //            links: [
        //                { rel: 'canonical', href: 'http://blogs.example.com/counter/something' },
        //                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/counter' }
        //            ]
        //        }
        //    },
        //    {
        //        path: 'users', component: UsersComponent,
        //        data: {
        //            title: 'Users REST example',
        //            meta: [{ name: 'description', content: 'This is User REST API example page Description!' }],
        //            links: [
        //                { rel: 'canonical', href: 'http://blogs.example.com/chat/something' },
        //                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/users' }
        //            ]
        //        }
        //    },
        //    {
        //        path: 'ngx-bootstrap', component: NgxBootstrapComponent,
        //        data: {
        //            title: 'Ngx-bootstrap demo!!',
        //            meta: [{ name: 'description', content: 'This is an Demo Bootstrap page Description!' }],
        //            links: [
        //                { rel: 'canonical', href: 'http://blogs.example.com/bootstrap/something' },
        //                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/bootstrap-demo' }
        //            ]
        //        }
        //    },

        //    { path: 'lazy', loadChildren: './containers/lazy/lazy.module#LazyModule'},

        //    {
        //        path: '**', component: NotFoundComponent,
        //        data: {
        //            title: '404 - Not found',
        //            meta: [{ name: 'description', content: '404 - Error' }],
        //            links: [
        //                { rel: 'canonical', href: 'http://blogs.example.com/bootstrap/something' },
        //                { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/bootstrap-demo' }
        //            ]
        //        }
        //    }
        //], {
        //  // Router options
        //  useHash: false,
        //  preloadingStrategy: PreloadAllModules,
        //  initialNavigation: 'enabled'
        //})
    ],
    providers: [
        LinkService,
        UserService,
        TranslateModule,
      AppService,
      Configuration,
      ResizeService
    ],
    bootstrap: [AppComponent]
})
export class AppModuleShared {
}
