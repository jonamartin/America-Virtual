import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import {SessionService} from '../services/session.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit{
  constructor(private formBuilder: FormBuilder,
              private httpClient: HttpClient,
              private sessionService: SessionService)
    {}
    public busqueda : FormGroup;
    public currentResult : any;
    public forecastResult: any;
    public country: string = 'Argentina';
    public city: string = 'Buenos Aires';
    public isLoggedIn : boolean = this.sessionService.isLoggedIn();
    headers =  new HttpHeaders()
    .set('Content-type', 'application/json')
    .set('Authorization', `Bearer ${this.sessionService.getToken()}`)


    public search()
    {
      let busquedaData = this.busqueda.getRawValue();
      this.country = encodeURIComponent(busquedaData.country);
      this.city = encodeURIComponent(busquedaData.city);
      this.httpClient.get<any>(`https://localhost:44347/WeatherForecast/Current?location=${this.city},${this.country}`, {headers: this.headers})
      .subscribe((res : Response) => {
        this.currentResult = res;
        this.httpClient.get<any>(`https://localhost:44347/WeatherForecast/Forecast?latitude=${this.currentResult.latitude}&longitude=${this.currentResult.longitude}`, {headers: this.headers})
        .subscribe((res : Response) => {
          this.forecastResult = res;
          this.currentResult.rainProbability = this.forecastResult[0].rainProbability;
          this.forecastResult = this.forecastResult.slice(1);
          this.currentResult.country = busquedaData.country;
          this.currentResult.city = busquedaData.city;
        });
      });
    }

    ngOnInit(): void 
    {
      this.busqueda = this.formBuilder.group({
        country : this.country,
        city: this.city
      });
      this.search();
      this.busqueda.reset();      
    }
}
