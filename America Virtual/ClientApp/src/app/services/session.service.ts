import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  constructor() { }

  saveToken(token: string)
  {
      localStorage.setItem("jwt", token);
  }

  removeToken()
  {
    localStorage.removeItem("jwt");
  }
  isLoggedIn()
  {
    return localStorage.getItem("jwt") !== null
  }
  getToken()
  {
    return localStorage.getItem("jwt");
  }
}