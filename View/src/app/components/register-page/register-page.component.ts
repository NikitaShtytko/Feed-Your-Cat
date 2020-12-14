import {Component, OnInit} from '@angular/core';
import {AsyncValidatorFn, FormControl, FormGroup, Validators} from "@angular/forms";
import {debounceTime, distinctUntilChanged, first, map, switchMap} from "rxjs/operators";
import {User} from "../../models/user";
import {UserService} from "../../services/user/user.service";
import {Subscription} from "rxjs";
import {Router} from "@angular/router";

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent implements OnInit {

  error: string;
  user: User;
  public subscriptions: Subscription[] = [];


  form: FormGroup = new FormGroup({
    login: new FormControl('', [
      Validators.required,
      Validators.pattern('^[a-zA-Zа-яА-Я\'_0-9]{4,40}$'),
    ]),

    email: new FormControl('', [
      Validators.required,
      Validators.email,
    ]),

    password: new FormControl('', [
      Validators.required,
      Validators.pattern('^[a-zA-Zа-яА-Я\'_0-9]{4,40}$')
    ], [
      this.emailValidator()
    ]),
  });

  constructor(private userService: UserService,
              private router: Router) {
  }

  ngOnInit(): void {
  }

  register() {
    this.user.name = this.form.controls.login.value;
    this.user.password = this.form.controls.password.value;
    this.user.email = this.form.controls.email.value;

    this.subscriptions.push(this.userService.register(this.user).subscribe(response => {
        this.router.navigate(['login']);
      },
      error => {
        this.error = error;
      },
    ));
  }

  private emailValidator(): AsyncValidatorFn {
    return control => control.valueChanges
      .pipe(
        debounceTime(500),
        distinctUntilChanged(),
        switchMap((val: string) => this.userService.existEmail(val)),
        map((res: User) => (res != null ? {emailExist: true} : null)),
        first()
      );
  }

  // private emailValidator(): AsyncValidatorFn {
  //   return control => control.valueChanges
  //     .pipe(
  //       debounceTime(500),
  //       distinctUntilChanged(),
  //       switchMap(_ => this.userService.existEmail(control.value)
  //         .pipe(
  //           map(result => result != null ? {emailExist: true} : null)
  //         )),
  //     );
  // }
}
