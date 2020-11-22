import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import Util from '../../shared/utils';
import { samePasswordValidator } from '../../shared/validators/samePasswordValidator';
import { RegisterUserDto, UserService } from '../../api/generated';

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

    registerForm = new FormGroup({
        firstName: new FormControl('', Validators.required),
        lastName: new FormControl('', Validators.required),
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', [Validators.required, Validators.pattern(Util.passwordRegex)]),
        passwordAgain: new FormControl('', Validators.required)
    }, { validators: samePasswordValidator });

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
                // Clear form
                this.registerForm.reset();
                alert('Successful registration');
            })
            .catch((response) => {
                // Conflict
                if (response.status === 409) {
                    alert(response.error.message);
                }
                // Bad request
                if (response.status === 400) {
                    alert('Registration failed');
                } else {
                    alert('Something went wrong. Please try again later.');
                }
            });
    }
}
