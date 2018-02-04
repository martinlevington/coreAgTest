import { NgModule } from '@angular/core';
import { Routes, RouterModule, PreloadAllModules } from '@angular/router';
import { HomeComponent } from './containers/home/home.component';
import { BasicLayoutComponent } from './components/basic-layout/basic-layout.component';
import { PlotlyComponent } from './shared/components/plotly/plotly.component';

const routes: Routes = [
  { path: '', redirectTo: 'home', pathMatch: 'full' },
  { path: '', component: BasicLayoutComponent,
     children: [
       {
         path: 'home',
         component: HomeComponent,
         // *** SEO Magic ***
         // We're using "data" in our Routes to pass in our <title> <meta> <link> tag information
         // Note: This is only happening for ROOT level Routes, you'd have to add some additional logic if you wanted this for Child level routing
         // When you change Routes it will automatically append these to your document for you on the Server-side
         //  - check out app.component.ts to see how it's doing this
         data: {
           title: 'Homepage',
           meta: [{ name: 'description', content: 'This is an example Description Meta tag!' }],
           links: [
             { rel: 'canonical', href: 'http://blogs.example.com/blah/nice' },
             { rel: 'alternate', hreflang: 'es', href: 'http://es.example.com/' }
           ]
         }
       },
        {
        path: 'plotly', component: PlotlyComponent
       }

       
      ]
  }

  ];

@NgModule({
  imports: [RouterModule.forRoot(routes,
    {
      useHash: false,
      preloadingStrategy: PreloadAllModules,
      initialNavigation: 'enabled'
    })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
