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

    getUserProfile(): void {
        this.userService.getUserProfile().subscribe(
            (res: UserProfileDto) => {
                this.profileDataForm.controls.firstName.setValue(res.firstName);
                this.profileDataForm.controls.lastName.setValue(res.lastName);
            },
            (err) => {
                // 401, 403, 500 if unauthorized, redirect to error
                this.router.navigate(['/error']);
            });
    }

    onEditProfile(): void {
        this.editingInProgress = true;
        this.profileDataForm.enable();
    }

    onSaveProfile(): void {
        this.editingInProgress = false;
        this.profileDataForm.disable();
        // TODO: edit profile
    }

    onDeleteAccount(): void {
        // TODO: delete account
    }

    onCancelEditing(): void {
        this.editingInProgress = false;
        this.profileDataForm.disable();
    }

    onCancelChangePassword(): void {
        this.passwordChangeInProgress = false;
        this.passwordChangeForm.disable();
    }

    onChangePassword(): void {
        this.passwordChangeInProgress = true;
        this.passwordChangeForm.enable();
    }

    onSavePassword(): void {
        this.passwordChangeInProgress = false;
        this.passwordChangeForm.disable();
    }

}
