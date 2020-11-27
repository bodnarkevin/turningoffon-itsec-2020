import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

export interface FilterData {
    title: string;
    tags: string;
    creator: string;
    uploader: string;
    date: Date;
}

@Component({
    selector: 'app-filter-caffs-dialog',
    templateUrl: './filter-caffs-dialog.component.html',
    styleUrls: ['./filter-caffs-dialog.component.css'],
})
export class FilterCaffsDialogComponent {
    filterCaffForm = new FormGroup({
        title: new FormControl(''),
        uploader: new FormControl(''),
        date: new FormControl(null),
    });

    disabled: boolean;

    constructor(
        public dialogRef: MatDialogRef<FilterCaffsDialogComponent>,
    ) { }

    onCancelClick(): void {
        this.dialogRef.close();
    }
}
