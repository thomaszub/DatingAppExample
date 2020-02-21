import { Injectable } from "@angular/core";
import * as alertify from "alertifyjs";

@Injectable({
  providedIn: "root"
})
export class AlertifyService {
  private title = "Dating App";
  constructor() {}

  confirm(message: string, okCallback: () => any) {
    alertify.confirm(message, (e: any) => {
      if (e) {
        okCallback();
      }
    }).set({title: this.title});
  }

  success(message: string) {
    alertify.success(message);
  }

  error(message: string) {
    alertify.error(message);
  }

  warning(message: string) {
    alertify.warning(message);
  }

  message(message: string) {
    alertify.message(message);
  }
}
