import {Component, OnInit} from '@angular/core';
import {RegisterUserDto, UserService} from '../api/generated';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  public message: string;

  constructor(private userService: UserService) {
  }

  ngOnInit(): void {
  }

  register(email: string, firstName: string, lastName: string, password: string): void {
    const registerUserDto: RegisterUserDto = {
      email,
      firstName,
      lastName,
      password
    };

    this.userService.registerUser(registerUserDto).toPromise()
      .then(() => {
        this.message = 'User registered!';
      }).catch(response => {
      this.message = JSON.stringify(response);
    });
  }

}
