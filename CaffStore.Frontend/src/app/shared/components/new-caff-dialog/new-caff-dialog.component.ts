import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CaffItemService } from '../../../api/generated';

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
        cafffile: new FormControl('', Validators.required),
    });

    caffFileName: string;

    constructor(
        public dialogRef: MatDialogRef<NewCaffDialogComponent>,
        private caffService: CaffItemService,
        @Inject(MAT_DIALOG_DATA) public data: DialogData,
    ) { }

    onCancelClick(): void {
        this.dialogRef.close();
        console.log(this.newCaffForm.controls.title.value);
    }

    csvInputChange(fileInputEvent: any): void {
        console.log(typeof fileInputEvent.target.files[0]);
        this.caffFileName = fileInputEvent.target.files[0].name;
    }
}
