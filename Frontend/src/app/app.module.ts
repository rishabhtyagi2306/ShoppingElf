import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule} from '@angular/Forms';
import { AppComponent } from './app.component';
import { SignupComponent } from './signup/signup.component';
import { ServerService } from './services/server.service';
import { NavbarComponent } from './navbar/navbar.component';
import { LoginComponent } from './login/login.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { HomePageComponent } from './home-page/home-page.component';
import { ProductsComponent } from './products/products.component';
import { ProductItemComponent } from './products/product-item/product-item.component';
import { ProductService } from './services/product.service';
import { ProductDetailsComponent } from './product-details/product-details.component';
import { ProductBagComponent } from './product-bag/product-bag.component';
import { ProductBagService } from './services/product-bag.service';
import { ProductBagItemComponent } from './product-bag/product-bag-item/product-bag-item.component';


@NgModule({
  declarations: [
    AppComponent,
    SignupComponent,
    NavbarComponent,
    LoginComponent,
    HomePageComponent,
    ProductsComponent,
    ProductItemComponent,
    ProductDetailsComponent,
    ProductBagComponent,
    ProductBagItemComponent,
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,


    
  ],
  providers: [ServerService,ProductService,ProductBagService],
  bootstrap: [AppComponent]
})
export class AppModule { }
