import {Component, OnInit} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {UserService} from "../../services/user/user.service";
import {Router} from "@angular/router";
import {CookieService} from "../../services/cookie/cookie.service";

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.css']
})
export class LoginPageComponent implements OnInit {

  error: string;

  form: FormGroup = new FormGroup({
    email: new FormControl('', [
      Validators.required,
      Validators.email,
    ]),

    password: new FormControl('', [
      Validators.required,
      Validators.pattern('^[a-zA-Zа-яА-Я\'_0-9]{7,40}$')
    ]),
  });

  constructor(
    private userService: UserService,
    private router: Router,
    private _authCookie: CookieService) {
  }

  ngOnInit(): void {
  }

  login() {

    const user = {
      email: this.form.controls.email.value,
      password: this.form.controls.password.value,
    };

    this.userService.login(user)
      .subscribe(
        (data: any) => {
          JSON.stringify(data);
          this._authCookie.setAuth(data.data.token);

          if (data.data.role === 'admin'){
            this.router.navigate(['/admin']);
          }
          else {
            this.router.navigate(['']);
          }
        },
        error => {
          this.error = 'Incorrect Login Or Password';
        },
      );
  }

}
