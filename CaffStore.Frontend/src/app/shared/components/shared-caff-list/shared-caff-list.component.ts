import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import {
  CaffItemDetailsDto,
  CaffItemDto,
  CaffItemDtoPagedResponse,
  CaffItemService,
} from '../../../api/generated';
import { NewCaffDialogComponent } from '../new-caff-dialog/new-caff-dialog.component';
import { AuthService } from 'src/app/auth/auth.service';
import { FilterCaffsDialogComponent } from '../filter-caffs-dialog/filter-caffs-dialog.component';

interface Option {
  value: string;
  viewValue: string;
}

interface Filter {
  value: string;
  name: string;
}

@Component({
  selector: 'app-shared-caff-list',
  templateUrl: './shared-caff-list.component.html',
  styleUrls: ['./shared-caff-list.component.css'],
})
export class SharedCaffListComponent implements OnInit {
  // list, own
  @Input() type: string;

  caffs: CaffItemDto[];

  /** Current page */
  page = 1;
  /** Total page count */
  pageCount = 1;
  filters: Filter[] = [];
  visible = true;

  selectable = false;
  removable = false;
  addOnBlur = true;

  newCaffTitle: string;
  newCaffDesc: string;
  loggedInUserEmail: string;
  options: Option[] = [
    { value: 'steak-0', viewValue: 'Steak' },
    { value: 'pizza-1', viewValue: 'Pizza' },
    { value: 'tacos-2', viewValue: 'Tacos' },
  ];

  constructor(
    private router: Router,
    private caffItemService: CaffItemService,
    public dialog: MatDialog,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.getCaffItems();
    console.log('page type onInit: ' + this.type);
    this.authService.getCurrentUserEmail().then((res) => {
      if (res) {
        this.loggedInUserEmail = res;
      }
    });
  }

  onViewDetails(caffId: number): void {
    this.router.navigate(['/caff'], {
      queryParams: {
        caffId,
      },
    });
  }

  getCaffItems(): void {
    this.caffItemService.getPagedCaffItems(this.page, 10).subscribe(
      (res: CaffItemDtoPagedResponse) => {
        if (this.page === 1) {
          if (this.type === 'list') {
            this.caffs = res.results;
          } else if (this.type === 'own') {
            this.caffs = res.results.filter(
              (r) => r.createdBy && r.createdBy.email === this.loggedInUserEmail
            );
          }
          this.pageCount = res.totalPageCount;
        } else {
          this.caffs = [...this.caffs, ...res.results];
        }
      },
      (err) => {
        alert('Something went wrong. Please try again later.');
      }
    );
  }

  onLoadMore(): void {
    if (this.page !== this.pageCount) {
      this.page += 1;
      this.getCaffItems();
    }
  }

  onAddNewCaff(): void {
    const dialogRef = this.dialog.open(NewCaffDialogComponent, {
      width: '600px',
      data: { title: this.newCaffTitle, descripton: this.newCaffDesc },
    });

    dialogRef.afterClosed().subscribe((result) => {
      console.log('The dialog was closed');
      if (result && result.controls) {
        const title = result.controls.title.value;
        const description = result.controls.description.value;

        this.caffItemService
          .addCaffItem(
            title,
            description,
            result.controls.cafffile.value as Blob
          )
          .subscribe(
            (res: CaffItemDetailsDto) => {
              this.getCaffItems();
            },
            (err) => {
              alert('Caff data upload failed');
            }
          );
      }
    });
  }

  onClearFilters(): void {
    this.filters = [];
    this.getCaffItems();
  }

  onFilterCaffs(): void {
    const dialogRef = this.dialog.open(FilterCaffsDialogComponent, {
      width: '600px',
      data: { title: this.newCaffTitle, descripton: this.newCaffDesc },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result && result.controls) {
        const title: string = result.controls.title.value;
        const uploader: string = result.controls.uploader.value;
        const date: Date = result.controls.date.value;

        if (title && title !== '') {
          this.caffs = this.caffs.filter((caff) => caff.title === title);
          this.filters.push({ name: 'title', value: title });
        }

        if (uploader && uploader !== '') {
          this.caffs = this.caffs.filter(
            (caff) => caff.createdBy.fullName === uploader
          );
          this.filters.push({ name: 'uploader', value: uploader });
        }

        if (date) {
          const months = date.getMonth() + 1;
          const monthsString =
            months < 10
              ? months.toString().padStart(2, '0')
              : months.toString();
          const days = date.getDate();
          const daysString =
            days < 10 ? days.toString().padStart(2, '0') : days.toString();
          const datestring =
            date.getFullYear() + '-' + monthsString + '-' + daysString;

          this.filters.push({ name: 'date', value: datestring });
          this.caffs = this.caffs.filter((caff) =>
            caff.lastModifiedAt.startsWith(datestring)
          );
        }
      }
    });
  }
}
