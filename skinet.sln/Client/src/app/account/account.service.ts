import { HttpClient } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { BehaviorSubject, ReplaySubject, map, of } from 'rxjs';
import { IUser } from '../shared/models/user';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService implements OnInit {
  baseUrl = 'https://localhost:5001/api/';

  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  ngOnInit(): void {
  }

  loadCurrentUser(token: string) {
    if (token === null) {
      this.currentUserSource.next(null);
      return of(null);
    }
    const headers = { 'Authorization': 'Bearer ' + token };
    const endPoint = this.baseUrl + 'account';
    return this.http.get(endPoint, { headers })
      .pipe(
        map((user: IUser) => {
          if (user) {
            localStorage.setItem('token', user.token);
            this.currentUserSource.next(user);
          }
        }
      ));
  }

  login(payload: any) {
    const endPoint = this.baseUrl + 'account/login';
    return this.http.post(endPoint, payload)
      .pipe(
        map((user: IUser) => {
          if (user) {
            localStorage.setItem('token', user.token);
            this.currentUserSource.next(user);
          }
        }
      ));
  }

  register(payload: any) {
    const endPoint = this.baseUrl + 'account/register';
    return this.http.post(endPoint, payload)
      .pipe(
        map((user: IUser) => {
          if (user) {
            localStorage.setItem('token', user.token);
            this.currentUserSource.next(user);
          }
        }
      ));
  }

  logout() {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  checkEmailExists(email: string) {
    const endPoint = this.baseUrl + 'account/emailexists?email=';
    return this.http.get(endPoint + email);
  }
}
