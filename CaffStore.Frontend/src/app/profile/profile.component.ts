import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { UserProfileDto, UserService } from '../api/generated';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnDestroy {

    profileDataForm = new FormGroup({
        firstName: new FormControl('', Validators.required),
        lastName: new FormControl('', Validators.required)
    });

    passwordChangeForm = new FormGroup({
        email: new FormControl(''),
        currentPassword: new FormControl('', Validators.required),
        newPassword: new FormControl('', Validators.required)
    });

    editingInProgress: boolean = false;
    passwordChangeInProgress: boolean = false;
    queryParamSubscription: Subscription = Subscription.EMPTY;

    // ha van userId, akkor az azt jelenti, hogy az adott id-jű felhasználó profilját tekintjük meg
    userId: number = null;

    constructor(private route: ActivatedRoute, private userService: UserService, private router: Router) { }

    ngOnInit() {
        // TODO: csak admin
        this.queryParamSubscription = this.route.queryParams.subscribe((params: Params) => {
            if (params && params.userId) {
                this.userId = params.userId;
            } else {
                this.userId = null;
            }
        });

        this.getUserProfile();
        this.profileDataForm.disable();
        this.passwordChangeForm.disable();
    }

    ngOnDestroy() {
        this.queryParamSubscription.unsubscribe();
    }

    /** get user profile */
    getUserProfile(): void {
        this.userService.getMyUserProfile().subscribe(
            (res: UserProfileDto) => {
                this.profileDataForm.controls.firstName.setValue(res.firstName);
                this.profileDataForm.controls.lastName.setValue(res.lastName);
                this.passwordChangeForm.controls.email.setValue(res.email);
            },
            (err) => {
                // 401, 403, 500 if unauthorized, redirect to error
                this.router.navigate(['/error']);
            });
    }

    /** Start user profile editing (enable form) */
    onEditProfile(): void {
        this.editingInProgress = true;
        this.profileDataForm.enable();
    }

    /** Save user data */
    onSaveProfile(): void {
        const userData = {
            firstName: this.profileDataForm.controls.firstName.value,
            lastName: this.profileDataForm.controls.lastName.value
        }
        this.userService.updateMyUserProfile(userData).subscribe(
            (res: UserProfileDto) => {
                this.editingInProgress = false;
                this.profileDataForm.disable();
            },
            (err) => {
                alert('Personal data change failed');
            })
    }

    /** Delete account */
    onDeleteAccount(): void {
        // TODO: delete account
    }

    /** Cancel profile data editing */
    onCancelEditing(): void {
        this.editingInProgress = false;
        this.profileDataForm.disable();
    }

    /** Cancel passwrod change */
    onCancelChangePassword(): void {
        this.passwordChangeInProgress = false;
        this.passwordChangeForm.disable();
    }

    /** Start change password (enable form) */
    onChangePassword(): void {
        this.passwordChangeInProgress = true;
        this.passwordChangeForm.controls.currentPassword.enable();
        this.passwordChangeForm.controls.newPassword.enable();
    }

    /** Change password */
    onSavePassword(): void {
        this.passwordChangeInProgress = false;
        this.passwordChangeForm.disable();
        // TODO: change password
    }
}
