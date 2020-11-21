import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnDestroy {

    form = new FormGroup({
        firstName: new FormControl('', Validators.required),
        lastName: new FormControl('', Validators.required),
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', Validators.required)
    });
    editingInProgress: boolean = false;
    queryParamSubscription: Subscription = Subscription.EMPTY;

    // ha van userId, akkor az azt jelenti, hogy az adott id-jű felhasználó profilját tekintjük meg
    userId: number = null;

    constructor(private route: ActivatedRoute) { }

    ngOnInit() {
        // TODO: csak admin
        this.queryParamSubscription = this.route.queryParams.subscribe((params: Params) => {
            if (params && params.userId) {
                this.userId = params.userId;
            } else {
                this.userId = null;
            }
        })
    }

    ngOnDestroy() {
        this.queryParamSubscription.unsubscribe();
    }

    onSaveProfile(): void {
        this.editingInProgress = false;
        // TODO: edit profile
    }

    onDeleteAccount(): void {
        // TODO: delete account
    }

}
