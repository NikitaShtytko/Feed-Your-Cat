import { Component, OnInit } from '@angular/core';
import {AsyncValidatorFn, FormControl, FormGroup, Validators} from "@angular/forms";
import {debounceTime, distinctUntilChanged, first, map, switchMap} from "rxjs/operators";
import {User} from "../../models/user";
import {UserService} from "../../services/user/user.service";

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.css']
})
export class RegisterPageComponent implements OnInit {

  error: string;

  form: FormGroup = new FormGroup({
    login: new FormControl('', [
      Validators.required,
      Validators.pattern('^[a-zA-Zа-яА-Я\'_0-9]{4,40}$')
    ]),

    email: new FormControl('', [
      Validators.required,
      Validators.email,
      this.emailValidator()
    ]),

    password: new FormControl('', [
      Validators.required,
      Validators.pattern('^[a-zA-Zа-яА-Я\'_0-9]{4,40}$')
    ]),
  });

  constructor(private userService: UserService) { }

  ngOnInit(): void {
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
}
