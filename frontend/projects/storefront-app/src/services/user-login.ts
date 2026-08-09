import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class UserLogin {
  private _isLoggedIn: boolean = false;
  private httpclient = inject(HttpClient);
  private url: string = 'http://localhost:7048/api/I0001_UserLogin';


  public sendHttpRequest() {
    return this.httpclient.get(this.url).subscribe((response) => {
      console.log(response);
    });

  }
  private void() {

  }
}
