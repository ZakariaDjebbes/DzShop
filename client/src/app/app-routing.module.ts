import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ErrorTestComponent } from './core/error-test/error-test.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  {path: '', component: HomeComponent, data: { breadcrumb: 'Home' }},
  {path: 'error-test', component: ErrorTestComponent, data: { breadcrumb: 'Error Testing' }},
  {path: 'server-error', component: ServerErrorComponent, data: { breadcrumb: '500 Server Error' }},
  {path: 'not-found', component: NotFoundComponent, data: { breadcrumb: '404 Not Found Error' }},
  {path: 'shop', loadChildren: () => import('./shop/shop.module').then(mod => mod.ShopModule), data: { breadcrumb: 'Shop' }},
  {path: 'checkout', loadChildren: () => import('./checkout/checkout.module').then(mod => mod.CheckoutModule), data: { breadcrumb: 'checkout' }},
  {path: 'basket', loadChildren: () => import('./basket/basket.module').then(mod => mod.BasketModule), data: { breadcrumb: 'Basket' }},
  {path: '**', redirectTo: 'not-found', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
