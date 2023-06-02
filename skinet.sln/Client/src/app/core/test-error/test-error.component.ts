import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss'],
})
export class TestErrorComponent implements OnInit {
  baseUrl: string = 'https://localhost:5001/api/';
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
