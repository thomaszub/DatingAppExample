import {
  BrowserModule,
  HammerGestureConfig,
  HAMMER_GESTURE_CONFIG
} from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { HttpClientModule } from "@angular/common/http";
import { FormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { BsDropdownModule, TabsModule } from "ngx-bootstrap";
import { JwtModule } from "@auth0/angular-jwt";
import { NgxGalleryModule } from "ngx-gallery";

import { AppComponent } from "./app.component";
import { HomeComponent } from "./home/home.component";
import { NavComponent } from "./nav/nav.component";
import { RegisterComponent } from "./register/register.component";
import { AuthService } from "./_services/auth.service";
import { ErrorInterceptorProvider } from "./_services/error.interceptor";
import { MemberListComponent } from "./members/member-list/member-list.component";
import { ListsComponent } from "./lists/lists.component";
import { MessagesComponent } from "./messages/messages.component";
import { appRoutes } from "./routes";
import { UserService } from "./_services/user.service";
import { MemberCardComponent } from "./members/member-card/member-card.component";
import { MemberDetailComponent } from "./members/member-detail/member-detail.component";
import { MemberDetailResolver } from "./_resolvers/member-detail.resolver";
import { MemberListResolver } from "./_resolvers/member-list.resolver";

export function tokenGetter() {
  return localStorage.getItem("token");
}

export class CustomHammerConfig extends HammerGestureConfig {
  overrides = {
    pinch: { enable: false },
    rotate: { enable: false }
  };
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavComponent,
    RegisterComponent,
    MemberListComponent,
    ListsComponent,
    MessagesComponent,
    MemberCardComponent,
    MemberDetailComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    TabsModule.forRoot(),
    BsDropdownModule.forRoot(),
    NgxGalleryModule,
    RouterModule.forRoot(appRoutes),
    JwtModule.forRoot({
      config: {
        tokenGetter,
        whitelistedDomains: ["localhost:5000"],
        blacklistedRoutes: ["localhost:5000/api/auth"]
      }
    })
  ],
  providers: [
    AuthService,
    UserService,
    ErrorInterceptorProvider,
    MemberDetailResolver,
    MemberListResolver,
    {
      provide: HAMMER_GESTURE_CONFIG,
      useClass: CustomHammerConfig
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}
