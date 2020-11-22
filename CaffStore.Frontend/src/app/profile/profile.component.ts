import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AdminUserService, ChangePasswordDto, UserProfileDto, UserService } from '../api/generated';

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

    // if there's a userId, we're looking at the given user's profile
    userId: number = null;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private userService: UserService,
        private adminUserService: AdminUserService) { }
    
    ngOnInit() {
        this.queryParamSubscription = this.route.queryParams.subscribe((params: Params) => {
            if (params && params.userId) {
                this.userId = params.userId;
                this.getUserProfileById();
            } else {
                this.userId = null;
                this.getUserProfile();
            }
        });

        this.profileDataForm.disable();
        this.passwordChangeForm.disable();
    }

    ngOnDestroy() {
        this.queryParamSubscription.unsubscribe();
    }

    /** Get user profile by ID */
    getUserProfileById(): void {
        this.adminUserService.getUserProfile(this.userId).subscribe(
            (res: UserProfileDto) => {
                this.profileDataForm.controls.firstName.setValue(res.firstName);
                this.profileDataForm.controls.lastName.setValue(res.lastName);
                this.passwordChangeForm.controls.email.setValue(res.email);
            },
            (err) => {
                if (err.status === 404) {
                    alert('User not found.')
                    this.router.navigate(['/users']);
                } else {
                    // 401, 403, 500 if unauthorized, redirect to error
                    this.router.navigate(['/error']);
                }
            });
    }

    /** Get user profile */
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

        if (this.userId) {
            // TODO: update profile by ID
        } else {
            this.userService.updateMyUserProfile(userData).subscribe(
                (res: UserProfileDto) => {
                    this.editingInProgress = false;
                    this.profileDataForm.disable();
                },
                (err) => {
                    alert('Personal data change failed');
                });
        }
    }

    /** Cancel profile data editing */
    onCancelEditing(): void {
        this.editingInProgress = false;
        this.profileDataForm.disable();
    }

    /** Start change password (enable form) */
    onChangePasswordClicked(): void {
        this.passwordChangeInProgress = true;
        this.passwordChangeForm.controls.currentPassword.enable();
        this.passwordChangeForm.controls.newPassword.enable();
    }

    /** Change password */
    onSavePassword(): void {
        const changePwData: ChangePasswordDto = {
            currentPassword: this.passwordChangeForm.controls.currentPassword.value,
            newPassword: this.passwordChangeForm.controls.newPassword.value
        }

        if (this.userId) {
            // TODO: change password by ID
        } else {
            this.userService.changeMyPassword(changePwData).subscribe(
                (res) => {
                    this.passwordChangeInProgress = false;
                    this.passwordChangeForm.disable();
                    this.passwordChangeForm.controls.currentPassword.setValue('');
                    this.passwordChangeForm.controls.newPassword.setValue('');
                },
                (err) => {
                    if (err.error.title) {
                        alert (err.error.title);
                    } else {
                        alert('Password change failed');
                    }
                });
        }
    }

    
    /** Cancel password change */
    onCancelChangePassword(): void {
        this.passwordChangeInProgress = false;
        this.passwordChangeForm.disable();
    }
    
    /** Delete account */
    onDeleteAccount(): void {
        // TODO: delete account
        console.log('delete account');
    }
}
