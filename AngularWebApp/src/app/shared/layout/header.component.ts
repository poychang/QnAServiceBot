import {Component, OnInit} from '@angular/core';
import {Router} from '@angular/router';

import {User} from '../models';
import {UserService} from '../services';

@Component({
  selector: 'app-layout-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  constructor(private router: Router, private userService: UserService) {}

  title: String = 'QnA Service Bot';
  currentUser: User;

  ngOnInit() {
    this.userService.currentUser$.subscribe((userData) => { this.currentUser = userData; });
  }
  logout() {
    this.userService.purgeAuth();
    this.router.navigateByUrl('/');
  }
}
