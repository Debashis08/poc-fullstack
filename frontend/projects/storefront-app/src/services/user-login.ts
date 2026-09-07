import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class UserLogin {
  private _isLoggedIn: boolean = false;
  private httpclient = inject(HttpClient);
  private url: string = 'http://localhost:7048/api/CustomerLogin';
  private requestBody: any = {
    "name": "debashis",
    "email": "debashisnandi@gmail.com",
    "passwordHash": "87246358hhwsdfy39ther"
};


  public sendHttpRequest() {
    return this.httpclient.post(this.url, this.requestBody).subscribe((response) => {
      console.log(response);
    });

  }
  private void() {

  }
}
