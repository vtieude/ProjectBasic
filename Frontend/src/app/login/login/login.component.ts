import { Component, OnInit } from '@angular/core';
import { LoginServiceProxy, LoginViewModel } from 'src/app/shared/services/login-proxies';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(public loginService: LoginServiceProxy) { }

  ngOnInit() {
    const login = new LoginViewModel();
    login.email = 'admin@gmail.com';
    login.password = 'Admin@1223';
    this.loginService.validateLogin(login).subscribe(res => {
    });
  }
}
