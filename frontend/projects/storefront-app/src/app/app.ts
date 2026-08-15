import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { UserLogin } from '../services/user-login';

@Component({
  selector: 'app-root',
  // imports: [RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  protected readonly title = signal('storefront-app');
  userLogin: UserLogin;

  constructor() {
    this.userLogin = inject(UserLogin);
  }
}
