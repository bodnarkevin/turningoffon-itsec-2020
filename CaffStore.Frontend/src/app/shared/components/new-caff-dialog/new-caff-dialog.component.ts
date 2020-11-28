import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { CaffItemDetailsDto, CaffItemService } from '../../../api/generated';

export interface DialogData {
  title: string;
  description: string;
}

@Component({
  selector: 'app-new-caff-dialog',
  templateUrl: './new-caff-dialog.component.html',
  styleUrls: ['./new-caff-dialog.component.css'],
})
export class NewCaffDialogComponent {
  newCaffForm = new FormGroup({
    title: new FormControl('', Validators.required),
    description: new FormControl('', Validators.required),
  });

  caffFileName: string;

  fileBlob: Blob;
  loading: boolean;

  constructor(
    public dialogRef: MatDialogRef<NewCaffDialogComponent>,
    private caffService: CaffItemService,
    private _snackBar: MatSnackBar,
    @Inject(MAT_DIALOG_DATA) public data: DialogData
  ) {}

  onCancelClick(): void {
    this.dialogRef.close();
  }

  onAddClick(): void {
    const input: HTMLInputElement = document.querySelector('input[type=file]');

    const file = input.files[0];
    this.newCaffForm.disable();
    this.loading = true;
    this.caffService
        .addCaffItem(
          this.newCaffForm.controls.title.value,
          this.newCaffForm.controls.description.value,
          file
        )
        .subscribe(
          (res: CaffItemDetailsDto) => {
            this.dialogRef.close();
            this.loading = false;
            this._snackBar.open(
                'Successfully uploaded new caff file!',
                null,
                {
                  duration: 3000,
                }
              );
          },
          (err) => {
            this.loading = false;
            this._snackBar.open(
              'Something went wrong during the upload. Please try again later!',
              null,
              {
                duration: 3000,
              }
            );
          }
        );

  }

  fileInputChange(fileInputEvent: any): void {
    this.caffFileName = fileInputEvent.target.files[0].name;
  }
}
