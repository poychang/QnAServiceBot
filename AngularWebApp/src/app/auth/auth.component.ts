import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, FormGroup, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {tap} from 'rxjs/operators';

import {UserService} from '../shared';

@Component({
  selector: 'app-auth-page',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.scss']
})
export class AuthComponent implements OnInit {
  error: String = '';
  authForm: FormGroup;

  constructor(private router: Router, private userService: UserService, private fb: FormBuilder) {
    this.authForm = this.fb.group(
      { 'username': ['', Validators.required], 'password': ['', Validators.required] });
  }

  ngOnInit() {}

  submitForm() {
    this.error = '';

    const credentials = this.authForm.value;
    this.userService.attemptAuth(credentials)
      .pipe(tap((user) => {
        if (user.role === 'admin') {
          this.router.navigateByUrl('/admin-chat-room');
        } else {
          this.router.navigateByUrl('/user-chat-room');
        }
      }))
      .subscribe(() => {}, err => {
        console.log(err);
        this.error = '登入失敗，請重新登入';
      });
  }
}
