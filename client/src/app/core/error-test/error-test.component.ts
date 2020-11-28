import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-error-test',
  templateUrl: './error-test.component.html',
  styleUrls: ['./error-test.component.scss']
})
export class ErrorTestComponent implements OnInit {
  baseUrl = environment.apiUrl;
  validationErrors: any;
  constructor(private http: HttpClient) { }

  ngOnInit(): void {
  }

  // tslint:disable-next-line: typedef
  get404Error() {
    return this.http.get(this.baseUrl + 'products/222').subscribe(response => {
      console.log(response);
    }, error => {
      console.error(error);
    });
  }

  // tslint:disable-next-line: typedef
  get500Error() {
    return this.http.get(this.baseUrl + 'buggy/servererror').subscribe(response => {
      console.log(response);
    }, error => {
      console.error(error);
    });
  }

  // tslint:disable-next-line: typedef
  get400Error() {
    return this.http.get(this.baseUrl + 'buggy/badrequest').subscribe(response => {
      console.log(response);
    }, error => {
      console.error(error);
    });
  }

    // tslint:disable-next-line: typedef
    get400ValError() {
      return this.http.get(this.baseUrl + 'products/twotwotwo').subscribe(response => {
        console.log(response);
      }, error => {
        console.error(error);
        this.validationErrors = error.errors;
      });
    }
}
