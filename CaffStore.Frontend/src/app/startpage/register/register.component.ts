import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { RegisterUserDto, UserService } from '../../api/generated';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

    public message: string;

    registerForm = new FormGroup({
        firstName: new FormControl('', Validators.required),
        lastName: new FormControl('', Validators.required),
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', Validators.required)
    });

    constructor(private userService: UserService) { }

    ngOnInit() { }


    onRegister(): void {
        const registerUserDto: RegisterUserDto = {
            email: this.registerForm.controls.email.value,
            firstName: this.registerForm.controls.firstName.value,
            lastName: this.registerForm.controls.lastName.value,
            password: this.registerForm.controls.password.value
        };
    
        this.userService.registerUser(registerUserDto).toPromise()
            .then(() => {
                this.message = 'User registered!';
            }).catch(response => {
                this.message = JSON.stringify(response);
            });
    }
}
