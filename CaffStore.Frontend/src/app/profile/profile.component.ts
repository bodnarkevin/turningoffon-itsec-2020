import {Component, OnInit} from '@angular/core';
import {UserProfileDto, UserService} from '../api-generated';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  public data: string;

  constructor(private userService: UserService) {
  }

  ngOnInit(): void {
    this.userService.getUserProfile().subscribe(userProfile => {
      this.data = JSON.stringify(userProfile);
    });
  }

}
