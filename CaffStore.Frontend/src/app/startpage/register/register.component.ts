import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

import Utils from '../../shared/utils';
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
        password: new FormControl('', [Validators.required, Validators.pattern(Utils.passwordRegex)]),
        passwordAgain: new FormControl('', Validators.required)
    }, { validators: samePasswordValidator });

    constructor(private userService: UserService, private _snackBar: MatSnackBar) { }

    ngOnInit(): void { }

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
                this._snackBar.open('Successful registration', null, {
                    duration: 2000,
                });
            })
            .catch((response) => {
                if (response.status === 400 || response.status === 409) {
                    this._snackBar.open(response.error.message, null, {
                        duration: 2000,
                    });
                } else {
                    this._snackBar.open('Something went wrong. Please try again later.', null, {
                        duration: 2000,
                    });
                }
            });
    }
}
