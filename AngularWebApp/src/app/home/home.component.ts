import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {Observable} from 'rxjs/Observable';

import {User, UserService} from '../shared';

@Component({
  selector: 'app-home-page',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  constructor(private router: Router, private userService: UserService) {}

  isAuthenticated: boolean;
  user$: Observable<User> = this.userService.currentUser$;

  ngOnInit() {
    this.userService.isAuthenticated$.subscribe(
      (authenticated) => { this.isAuthenticated = authenticated; },
      (error) => { console.log(`login first${error.message}`); });
  }

  enter() {
    this.user$.subscribe(user => {
      if (user.role === 'admin') {
        this.router.navigateByUrl('/admin-chat-room');
      } else {
        this.router.navigateByUrl('/user-chat-room');
      }
    });
  }
}
