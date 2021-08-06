import { Injectable } from '@angular/core';

import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Product } from '../shared/product.module';
import { ProductService } from './product.service';
import { AppComponent } from '../app.component';
import { Category1Service } from './category1.service';
import { Category2Service } from './category2.service';
import { SizeService } from './size.service';
import { Router } from '@angular/router';

@Injectable()

export class ServerService
{      
  tk:any;
  Products:Product[];
  body:{}; 
  public rootUrl="https://cb47b78c3e75.ngrok.io"
  


  constructor(private http :HttpClient,
              private productService:ProductService,
              private CategoryService:Category1Service ,
              private CategoryService2:Category2Service,
              private sizeService:SizeService,
              private appComponent:AppComponent,
              private router:Router
              ){}


  signup(yourName:string,phoneNumber:string,email:string,password:string,confirmPassword:string) 
   
    {  
      console.log(this.rootUrl+'/api/Account')
        const headers = new HttpHeaders({'Content-Type':'application/json'});
         console.log(JSON.stringify({yourName,email,password,confirmPassword,phoneNumber}));
        return this.http.post(this.rootUrl+'/api/Account',JSON.stringify({yourName,email,password,confirmPassword,phoneNumber}),
        {headers:headers})
       
    }

    login(email:string ,password:string)
  { 
    const headers = new HttpHeaders({'Content-Type':'application/json'});
    console.log(JSON.stringify({email,password}));
    console.log(this.rootUrl+'/api/Account/UserLogin')
   return this.http.post(this.rootUrl+'/api/Account/UserLogin',JSON.stringify({email,password}),{headers:headers})
   
    
  }

  sellersignup(yourName:string,mobileNumber:string,email:string,password:string,confirmPassword:string) 
   
    {  
      
        const headers = new HttpHeaders({'Content-Type':'application/json'});
        
       
        console.log(JSON.stringify({yourName,email,password,confirmPassword,mobileNumber}));
        return this.http.post(this.rootUrl+'/api/Seller/Signup',JSON.stringify({yourName,email,password,confirmPassword,mobileNumber}),
        {headers:headers})
        
    }


    sellerlogin(email:string ,password:string)
  { 
    const headers = new HttpHeaders({'Content-Type':'application/json'});
    console.log(JSON.stringify({email,password}));
    this.appComponent.loaders();
  return this.http.post(this.rootUrl+'/api/Seller/Login',JSON.stringify({email,password}),{headers:headers})
 
    
  }




 loggedIn()
  {
    return !!localStorage.getItem('token')
  }

  loggedIn2()
  {
    return !!localStorage.getItem('token2')
  } 
  getToken(){
    return localStorage.getItem('token')
  }
  loggOut()
  {
    localStorage.removeItem('token');

  }
  getallProducts(index:number)
  {

    console.log(this.rootUrl+'/api/Product/GetProductBySuitableID/'+index);
     return this.http.get( this.rootUrl+'/api/Product/GetProductBySuitableID/'+index)
    


  
  }

  GetCategory1(Index:number)
  {
    console.log(this.rootUrl+'/api/Product/Category/'+Index);
    this.appComponent.loaders();
  return  this.http.get( this.rootUrl+'/api/Product/Category/'+Index)
    

  }

  GetCategory2(index:number)
  {
    console.log(this.rootUrl+'/api/Product/SubCategory/'+index);
   return this.http.get( this.rootUrl+'/api/Product/SubCategory/'+index)
    

  }
  
  
  
  CategoryProducts(Index:number)
  {
    return this.http.get(this.rootUrl+'/api/Product/GetProduct/'+Index);
      
  }


  size(Index:number)
  {
    console.log(this.rootUrl+'/api/Product/ProductDetails/'+Index)
  return this.http.get(this.rootUrl+'/api/Product/ProductDetails/'+Index)
    
  }

  SellerDetails(AddressLine1,AddressLine2,pincode,
    state,city,AccountHolderName,accountNumber,IFSCCode,accountType,
    ShippingFee,GSTNumber,PANCardNumber)
    {
    
    
    const headers = new HttpHeaders({'Content-Type':'application/json'});
    console.log(JSON.stringify({AddressLine1,AddressLine2,pincode,state,city,AccountHolderName,accountNumber
                    ,IFSCCode,accountType,ShippingFee,GSTNumber,PANCardNumber}));
      return this.http.post(this.rootUrl+'/api/Seller/EnterDetails/?token='+localStorage.getItem('token2'),JSON.stringify({AddressLine1,AddressLine2,pincode,state,city,AccountHolderName,accountNumber,
   IFSCCode,accountType,ShippingFee,GSTNumber,PANCardNumber}),
      {headers:headers})
      
  }

  SendOtp(OTP:string)
  {   
    const headers = new HttpHeaders({'Content-Type':'application/json'});
    console.log(JSON.stringify({OTP}));
    return this.http.post(this.rootUrl+'/api/Seller/EnterOTP/'+localStorage.getItem('num'),JSON.stringify({OTP}), {headers:headers})
  }
  resendOtp()
  {
    const headers = new HttpHeaders({'Content-Type':'application/json'});
    return this.http.post(this.rootUrl+'/api/Seller/ResendOTP/'+localStorage.getItem('num'),this.body, {headers:headers})
  }

  myOrders()
  {
    
   return this.http.get(this.rootUrl+'/api/User/OrderList/?token='+localStorage.getItem('token'))
   
  }

  MyBag()
  {   
    
     console.log(this.rootUrl+'/api/User/GetCart/?token='+localStorage.getItem('token'))
   return this.http.get(this.rootUrl+'/api/User/GetCart/?token='+localStorage.getItem('token'))
    
   
 
    
  
  }

  Addtocart(pid:number)
  {
  
    const headers = new HttpHeaders({'Content-Type':'application/json'});
    console.log(this.rootUrl+'/api/User/AddtoCart/'+pid+'/?token='+localStorage.getItem('token'))
    return this.http.post(this.rootUrl+'/api/User/AddtoCart/'+pid+'/?token='+localStorage.getItem('token'),this.body,{headers:headers})
  }

  Delete(id:number)
  {
    console.log(id);
    const headers = new HttpHeaders({'Content-Type':'application/json'});
    console.log(this.rootUrl+'/api/User/Cart/'+id+'/?token='+localStorage.getItem('token'))
    return this.http.post(this.rootUrl+'/api/User/Cart/'+id+'/?token='+localStorage.getItem('token'),this.body,{headers:headers})
 
  }
  upQuantity(cid:number,Quantity:number)
  {

    const headers = new HttpHeaders({'Content-Type':'application/json'});
    console.log(this.rootUrl+'/api/User/UpdateCart/'+cid+'/?token='+localStorage.getItem('token'))
    console.log(JSON.stringify({Quantity}));
    return this.http.post(this.rootUrl+'/api/User/UpdateCart/'+cid+'/?token='+localStorage.getItem('token'),JSON.stringify({Quantity}),{headers:headers})
  }
  AllOrder()
  {
    const headers = new HttpHeaders({'Content-Type':'application/json'});
    console.log(this.rootUrl+'/api/User/OrderFromCart/?token='+localStorage.getItem('token'))
    return this.http.post(this.rootUrl+'/api/User/OrderFromCart/?token='+localStorage.getItem('token'),this.body,{headers:headers})
  }
  oneOrder(pid:number)
  {
    const headers = new HttpHeaders({'Content-Type':'application/json'});
    console.log(this.rootUrl+'/api/User/OrderNow/'+pid+'/?token='+localStorage.getItem('token'))
    return this.http.post(this.rootUrl+'/api/User/OrderNow/'+pid+'/?token='+localStorage.getItem('token'),this.body,{headers:headers})
  }
  address(AddressLine1:string,AddressLine2:string,pincode:number,city:string,State:string)
  {
    const headers = new HttpHeaders({'Content-Type':'application/json'});
    console.log(JSON.stringify({AddressLine1,AddressLine2,pincode,State,city}));
     console.log(this.rootUrl+'/api/User/Address/?token='+localStorage.getItem('token'))
     return this.http.post(this.rootUrl+'/api/User/Address/?token='+localStorage.getItem('token'),JSON.stringify({AddressLine1,AddressLine2,pincode,State,city}),{headers:headers})
  }
 
  sellerProduct()
  {
    
    return this.http.get(this.rootUrl+'/api/Product/Seller/Show/?token='+localStorage.getItem('token2'))

  }

  Selled()
  {
    return this.http.get(this.rootUrl+'/api/Seller/Show/OrderPlaced/?token='+localStorage.getItem('token2'))
    
  }


  addProduct(productName:string,productBrand:string,SuitableID:number,SubCategory:number,productDetails:string)
  {   
    const headers = new HttpHeaders({'Content-Type':'application/json'});
    console.log(JSON.stringify({productName,productBrand,SuitableID,SubCategory,productDetails}));
    return this.http.post(this.rootUrl+'/api/Product/AddProduct/'+SubCategory+'/'+SuitableID+'/?token='+localStorage.getItem('token2'),JSON.stringify({productName,productBrand,productDetails}),{headers:headers})
     
  }
  
  uplode( fd:FormData,index:number,productID:number)
  {
    return this.http.post(this.rootUrl+'/api/UploadImage/'+productID+'/'+index+'/?token='+localStorage.getItem('token2'),fd)
    
  }
  addSize(productId:number,productPrice:number,productSize,productQuantity)
  {
    const headers =new HttpHeaders({'Content-Type':'application/json'});
    return this.http.post(this.rootUrl +'/api/Product/AddSize/'+productId+'/?token='+localStorage.getItem('token2'),JSON.stringify({productPrice,productSize,productQuantity}),{headers:headers})
  }
   DeleteProduct(id:number)
  {  
    
    const headers = new HttpHeaders({'Content-Type':'application/json'});
    console.log(this.rootUrl+'/api/Product/Delete/'+id+'/?token='+localStorage.getItem('token2'))
    return this.http.post(this.rootUrl+'/api/Product/Delete/'+id+'/?token='+localStorage.getItem('token2'),this.body,{headers:headers})
   }

  deleteSize(id:number)
  {   console.log(id);
    const headers = new HttpHeaders({'Content-Type':'application/json'});
      console.log(this.rootUrl+'/api/Product/Delete/Size/'+id+'/?token='+localStorage.getItem('token2'))
    return this.http.post(this.rootUrl+'/api/Product/Delete/Size/'+id+'/?token='+localStorage.getItem('token2'),this.body,{headers:headers})
    
  } 

}
