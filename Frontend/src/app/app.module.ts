import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { LoginServiceProxy, API_LOGIN_URL } from './shared/services/login-proxies';
import { EnvService } from './shared/services/env.service';
import { LoginComponent } from './login/login/login.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule
  ],
  providers: [LoginServiceProxy, EnvService,
    { provide: API_LOGIN_URL, useFactory: getLoginUrl, deps: [EnvService] }],
  bootstrap: [AppComponent]
})
export class AppModule { }
export function getLoginUrl(env: EnvService): string {
  return env.apiLogin;
}
