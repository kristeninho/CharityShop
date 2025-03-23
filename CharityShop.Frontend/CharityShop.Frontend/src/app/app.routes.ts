import { Routes } from '@angular/router';
import { LoginComponent } from '../app/components/login/login.component';
import { ShopComponent } from '../app/components/shop/shop.component';
import { StockComponent } from '../app/components/stock/stock.component';
import { NavigationComponent } from '../app/components/navigation/navigation.component';
import { AuthGuard } from '../app/auth.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'navigation', component: NavigationComponent, canActivate: [AuthGuard] },
  { path: 'shop', component: ShopComponent, canActivate: [AuthGuard] },
  { path: 'stock', component: StockComponent, canActivate: [AuthGuard] },

  { path: '', redirectTo: '/login', pathMatch: 'full' }, // Default route (redirect to login)
  { path: '**', redirectTo: '/login' }, // Fallback route (redirect to login)
];