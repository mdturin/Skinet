import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss'],
})
export class TestErrorComponent implements OnInit {
  baseUrl = environment.apiUrl;
  validationErrors: string[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {}

  get404Error() {
    const endPoint = this.baseUrl + 'buggy/notfound';
    this.http.get(endPoint).subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }

  get400Error() {
    const endPoint = this.baseUrl + 'buggy/badrequest';
    this.http.get(endPoint).subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }

  get500Error() {
    const endPoint = this.baseUrl + 'buggy/servererror';
    this.http.get(endPoint).subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }

  get401Error() {
    const endPoint = this.baseUrl + 'buggy/auth';
    this.http.get(endPoint).subscribe({
      next: (response) => console.log(response),
      error: (error) => console.log(error)
    });
  }

  get400ValidationError() {
    const endPoint = this.baseUrl + 'products/fortytwo';
    this.http.get(endPoint).subscribe({
      next: (response) => console.log(response),
      error: (error) => {
        console.log(error);
        this.validationErrors = error.errors;
      }
    });
  }
}
