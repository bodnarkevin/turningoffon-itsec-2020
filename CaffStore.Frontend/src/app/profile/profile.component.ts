import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
    selector: 'app-profile',
    templateUrl: './profile.component.html',
    styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

    editingInProgress: boolean = false;

    form = new FormGroup({
        firstName: new FormControl('', Validators.required),
        lastName: new FormControl('', Validators.required),
        email: new FormControl('', [Validators.required, Validators.email]),
        password: new FormControl('', Validators.required)
    });

    constructor() { }

    ngOnInit() { }

    onSaveProfile(): void {
        this.editingInProgress = false;
        // TODO: edit profile
    }

    onDeleteAccount(): void {
        // TODO: delete account
    }

}
