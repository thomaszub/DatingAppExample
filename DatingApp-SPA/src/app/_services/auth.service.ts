import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map } from "rxjs/operators";
import { JwtHelperService } from "@auth0/angular-jwt";
import { environment } from "../../environments/environment";
import {User} from "../_models/user";
import {BehaviorSubject} from "rxjs";

@Injectable({
  providedIn: "root"
})
export class AuthService {
  baseUrl = environment.apiUrl + "/auth";
  jwtHelper = new JwtHelperService();
  decodedToken: any;
  currentUser: User;
  photoUrl = new BehaviorSubject<string>("../../assets/user.png");
  currentPhotoUrl = this.photoUrl.asObservable();

  constructor(private http: HttpClient) {}

  changeMemberPhoto(photoUrl: string) {
    this.photoUrl.next(photoUrl);
  }

  login(model: any) {
    return this.http.post(this.baseUrl + "/login", model).pipe(
      map((response: any) => {
        const res = response;
        if (res) {
          localStorage.setItem("token", res.token);
          localStorage.setItem("user", JSON.stringify(res.user));
          this.decodedToken = this.jwtHelper.decodeToken(res.token);
          this.currentUser = res.user;
          this.changeMemberPhoto(this.currentUser.photoUrl);
        }
      })
    );
  }

  register(model: any) {
    return this.http.post(this.baseUrl + "/register", model);
  }

  loggedIn() {
    const token = localStorage.getItem("token");
    return !this.jwtHelper.isTokenExpired(token);
  }
}
