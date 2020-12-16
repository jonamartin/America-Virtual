import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import {SessionService} from '../services/session.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit{
  constructor(private formBuilder: FormBuilder,
              private httpClient: HttpClient,
              private sessionService: SessionService){}

  public formVisible: boolean = false;
  public loginForm : FormGroup;
  public isLoggedIn : boolean = this.sessionService.isLoggedIn();

  toggleForm() {
    this.formVisible = this.formVisible ? false : true;
  }

  logIn() {
    let loginFormData = this.loginForm.getRawValue()
    const data = {
      'Email': loginFormData.email,
      'Password': loginFormData.password
    }
    this.httpClient.post<any>(`https://localhost:44347/User/Login`, data)
    .subscribe(response => {
        const token = (<any>response).token;
        this.sessionService.saveToken(token);
        window.location.reload();
      }, error => {
        console.log(error) 
      },
    )
  }
  logOut(){
    this.sessionService.removeToken();
    window.location.reload();
  }
  ngOnInit(): void 
  {
    this.loginForm = this.formBuilder.group({
      email: '',
      password: ''
    })
  }
}