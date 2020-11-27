import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { OAuthService } from 'angular-oauth2-oidc';
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

    editingInProgress = false;
    passwordChangeInProgress = false;
    queryParamSubscription: Subscription = Subscription.EMPTY;

    // if there's a userId, we're looking at the given user's profile
    userId: number = null;
    isViewedUserAdmin: boolean = null;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private oAuthService: OAuthService,
        private userService: UserService,
        private adminUserService: AdminUserService,
        private _snackBar: MatSnackBar) { }

    ngOnInit(): void {
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

    ngOnDestroy(): void {
        this.queryParamSubscription.unsubscribe();
    }

    /** Get user profile by ID */
    getUserProfileById(): void {
        this.adminUserService.getUserProfile(this.userId).subscribe(
            (res: UserProfileDto) => {
                this.profileDataForm.controls.firstName.setValue(res.firstName);
                this.profileDataForm.controls.lastName.setValue(res.lastName);
                this.passwordChangeForm.controls.email.setValue(res.email);
                this.isViewedUserAdmin = res.roles.includes('Admin');
            },
            (err) => {
                if (err.status === 404) {
                    this._snackBar.open('User not found.', null, {
                        duration: 2000,
                    });
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
        };

        if (this.userId) {
            this.adminUserService.updateUserProfile(this.userId, userData).subscribe(
                (res) => {
                    this.editingInProgress = false;
                    this.profileDataForm.disable();
                },
                (err) => {
                    this._snackBar.open('Personal data change failed', null, {
                        duration: 2000,
                    });
                }
            )
        } else {
            this.userService.updateMyUserProfile(userData).subscribe(
                (res: UserProfileDto) => {
                    this.editingInProgress = false;
                    this.profileDataForm.disable();
                },
                (err) => {
                    this._snackBar.open('Personal data change failed', null, {
                        duration: 2000,
                    });
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
        // admin does not have to know the current password
        if (!this.userId) {
            this.passwordChangeForm.controls.currentPassword.enable();
        }

        this.passwordChangeForm.controls.newPassword.enable();
    }

    /** Change password */
    onSavePassword(): void {
        const changePwData: ChangePasswordDto = {
            currentPassword: this.passwordChangeForm.controls.currentPassword.value,
            newPassword: this.passwordChangeForm.controls.newPassword.value
        };

        if (this.userId) {
            this.adminUserService.changeUserPassword(this.userId, { newPassword: this.passwordChangeForm.controls.newPassword.value }).subscribe(
                (res) => {
                    this.passwordChangeInProgress = false;
                    this.passwordChangeForm.disable();
                    this.passwordChangeForm.controls.currentPassword.setValue('');
                    this.passwordChangeForm.controls.newPassword.setValue('');
                },
                (err) => {
                    if (err.error.title) {
                        this._snackBar.open(err.error.title, null, {
                            duration: 2000,
                        });
                    } else {
                        this._snackBar.open('Password change failed', null, {
                            duration: 2000,
                        });
                    }
                });
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
                        this._snackBar.open(err.error.title, null, {
                            duration: 2000,
                        });
                    } else {
                        this._snackBar.open('Password change failed', null, {
                            duration: 2000,
                        });
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
        if (this.userId) {
            this.adminUserService.deleteUserProfile(this.userId).subscribe(
                (res) => {
                    this.router.navigate(['/users']);
                },
                (err) => {
                    this._snackBar.open('Profile deletion failed!', null, {
                        duration: 2000,
                    });
                }
            )
        } else {
            this.userService.deleteMyUserProfile().subscribe(
                (res) => {
                    this.oAuthService.revokeTokenAndLogout().then(
                        () => {
                            this.router.navigate(['/']);
                        }
                    )
                },
                (err) => {
                    this._snackBar.open('Profile deletion failed!', null, {
                        duration: 2000,
                    });
                }
            )
        }
    }

    /** Grant admin role */
    onGrantAdminRole(): void {
        this.adminUserService.grantUserAdminRole(this.userId).subscribe(
            (res) => {
                this.profileDataForm.controls.firstName.setValue(res.firstName);
                this.profileDataForm.controls.lastName.setValue(res.lastName);
                this.passwordChangeForm.controls.email.setValue(res.email);
                this.isViewedUserAdmin = res.roles.includes('Admin');
                
                if (this.isViewedUserAdmin) {
                    this._snackBar.open('Role changed to admin.', null, {
                        duration: 2000,
                    });
                } else {
                    this._snackBar.open('Couldn\'t change role. Please try again later!', null, {
                        duration: 2000,
                    });
                }
            },
            (err) => {
                if (err.status === 404 || err.status === 400) {
                    this._snackBar.open(err.error.message, null, {
                        duration: 2000,
                    });
                } else if (err.status === 401 || err.status === 403) {
                    this._snackBar.open('You are not authorized to execute this operation!', null, {
                        duration: 2000,
                    });
                } else {
                    this._snackBar.open('Something went wrong, please try again later!', null, {
                        duration: 2000,
                    });
                }
                
            }
        )
    }
}
