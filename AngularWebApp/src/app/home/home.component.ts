import {Component, OnInit} from '@angular/core';
import {Observable} from 'rxjs/Observable';

import {User, UserService} from '../shared';

@Component({
  selector: 'app-home-page',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  constructor(private userService: UserService) {}

  isAuthenticated: boolean;
  user$: Observable<User> = this.userService.currentUser$;

  ngOnInit() {
    this.userService.isAuthenticated$.subscribe(
      (authenticated) => { this.isAuthenticated = authenticated; });
  }
}
