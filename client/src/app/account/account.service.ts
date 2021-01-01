import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { of, ReplaySubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IAddress } from '../shared/models/address';
import { IUser } from '../shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new ReplaySubject<IUser>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient, private router: Router) { }

  // tslint:disable-next-line: typedef
  public login(values: any) {
    return this.http.post<IUser>(this.baseUrl + 'account/login', values)
      .pipe(map((user: IUser) => {
        if (user) {
          localStorage.setItem('token', user.token);
          this.currentUserSource.next(user);
          return user;
        }
      }));
  }

  public logout(): void {
    localStorage.removeItem('token');
    this.currentUserSource.next(null);
    this.router.navigateByUrl('/');
  }

  // tslint:disable-next-line: typedef
  public checkEmailExists(email: string) {
    return this.http.get(this.baseUrl + 'account/emailExists?email=' + email);
  }

  // tslint:disable-next-line: typedef
  public checkUserExists(username: string) {
    return this.http.get(this.baseUrl + 'account/userExists?username=' + username);
  }

  // tslint:disable-next-line: typedef
  public register(values: any) {
    return this.http.post(this.baseUrl + 'account/register', values).pipe(map((user: IUser) => {
      if (user) {
        localStorage.setItem('token', user.token);
        this.currentUserSource.next(user);
      }
    }));
  }

  // tslint:disable-next-line: typedef
  public loadCurrentUser(token: string) {
    if (token === null) {
      this.currentUserSource.next(null);
      return of(null);
    }

    let headers = new HttpHeaders();
    headers = headers.set('Authorization', `Bearer ${token}`);

    return this.http.get(this.baseUrl + 'account ', { headers }).pipe(
      map((user: IUser) => {
        if (user) {
          this.currentUserSource.next(user);
          localStorage.setItem('token', user.token);
        }
      })
    );
  }

  // tslint:disable-next-line: typedef
  public getUserAddress() {
    return this.http.get<IAddress>(this.baseUrl + 'account/address');
  }

  // tslint:disable-next-line: typedef
  public updateUserAddress(address: IAddress) {
    return this.http.put<IAddress>(this.baseUrl + 'account/address', address);
  }

  // tslint:disable-next-line: typedef
  public updateUser(values: any) {
    return this.http.put<IUser>(this.baseUrl + 'account/updateProfile', values).pipe(map((user: IUser) => {
      if (user) {
        localStorage.setItem('token', user.token);
        this.currentUserSource.next(user);
        return user;
      }
    }));
  }

  // tslint:disable-next-line: typedef
  public updateUserPassword(values: any) {
    return this.http.put(this.baseUrl + 'account/updatePassword', values);
  }
}
