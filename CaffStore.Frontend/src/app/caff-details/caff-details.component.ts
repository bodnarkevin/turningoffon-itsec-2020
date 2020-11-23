import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-caff-details',
  templateUrl: './caff-details.component.html',
  styleUrls: ['./caff-details.component.css']
})
export class CaffDetailsComponent implements OnInit {

  caffDataForm = new FormGroup({
    fullName: new FormControl('', Validators.required),
  });

  editingInProgress = false;
  queryParamSubscription: Subscription = Subscription.EMPTY;

  constructor() { }

  ngOnInit(): void {
    this.caffDataForm.disable();
  }

  /** Start caff data editing (enable form) */
  onEditCaffData(): void {
    this.editingInProgress = true;
    this.caffDataForm.enable();
  }

  /** Save user data */
  onSaveChanges(): void {
    const caffData = {
        fullName: this.caffDataForm.controls.fullName.value,
    };

    this.editingInProgress = false;
    this.caffDataForm.disable();
    /*
    if (this.caffId) {
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
    */
  }

  /** Cancel profile data editing */
  onCancelEditing(): void {
      this.editingInProgress = false;
      this.caffDataForm.disable();
  }

}
