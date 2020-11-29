import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import {
  CaffItemDto,
  CaffItemDtoPagedResponse,
  CaffItemService,
} from '../../../api/generated';
import { NewCaffDialogComponent } from '../new-caff-dialog/new-caff-dialog.component';
import { AuthService } from 'src/app/auth/auth.service';
import { FilterCaffsDialogComponent } from '../filter-caffs-dialog/filter-caffs-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';

interface Option {
  value: OrderOption;
  name: string;
}

interface Filter {
  value: string;
  name: string;
}

enum OrderOption {
  TitleAscending = 'TitleAscending',
  TitleDescending = 'TitleDescending',
  DateAscending = 'DateAscending',
  DateDescending = 'DateDescending',
}

@Component({
  selector: 'app-shared-caff-list',
  templateUrl: './shared-caff-list.component.html',
  styleUrls: ['./shared-caff-list.component.css'],
})
export class SharedCaffListComponent implements OnInit {
  // list, own
  @Input() type: string;

  caffs: CaffItemDto[] = [];

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
    { value: OrderOption.DateAscending, name: 'Upload date - Ascending' },
    { value: OrderOption.DateDescending, name: 'Upload date - Descending' },
    { value: OrderOption.TitleAscending, name: 'Title - Ascending' },
    { value: OrderOption.TitleDescending, name: 'Title - Descending' },
  ];

  constructor(
    private router: Router,
    private caffItemService: CaffItemService,
    public dialog: MatDialog,
    public filterDialog: MatDialog,
    private authService: AuthService,
    private _snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.authService.getCurrentUserEmail().then((res) => {
      if (res) {
        this.loggedInUserEmail = res;
        this.getCaffItems();
        if (this.caffs.length < 5) {
          this.page++;
          this.getCaffItems();
        }
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
          this.caffs = res.results;
          if (this.type === 'own') {
            this.caffs = this.caffs.filter(
              (r) => r.createdBy && r.createdBy.email === this.loggedInUserEmail
            );
          }
          this.pageCount = res.totalPageCount;
        } else {
          this.caffs = [...this.caffs, ...res.results];
          if (this.type === 'own') {
            this.caffs = this.caffs.filter(
              (r) => r.createdBy && r.createdBy.email === this.loggedInUserEmail
            );
          }
          this.pageCount = res.totalPageCount;
        }
      },
      (err) => {
        if (err.status === 401 || err.status === 403) {
          this._snackBar.open(
            'You are not authorized to access this page!',
            null,
            {
              duration: 2000,
            }
          );
        } else {
          this._snackBar.open(
            'Something went wrong. Please try again later!',
            null,
            {
              duration: 2000,
            }
          );
        }
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
      disableClose: true,
      data: { title: this.newCaffTitle, descripton: this.newCaffDesc },
    });

    dialogRef.afterClosed().subscribe((result) => {
        this.getCaffItems();
    });
  }

  onClearFilters(): void {
    this.filters = [];
    this.getCaffItems();
  }

  onSelectionChange(event: any): void {
    const selectedValue: OrderOption =
      OrderOption[event.value];
    switch (selectedValue) {
      case OrderOption.DateAscending:
        this.orderDateAscending();
        break;

      case OrderOption.DateDescending:
        this.orderDateDescending();
        break;

      case OrderOption.TitleAscending:
        this.orderTitleAscending();
        break;

      case OrderOption.TitleDescending:
        this.orderTitleDescending();
        break;
    }
  }

  orderTitleDescending(): void {
    this.caffs.sort((a: CaffItemDto, b: CaffItemDto) => {
      if (!b || !a || !a.title || !b.title) {
        return 0;
      }
      if (b.title.toLowerCase() > a.title.toLowerCase()) {
        return 1;
      }
      if (b.title.toLowerCase() === a.title.toLowerCase()) {
        return 0;
      }
      return -1;
    });
  }

  orderTitleAscending(): void {
    this.caffs.sort((a: CaffItemDto, b: CaffItemDto) => {
      if (!b || !a || !a.title || !b.title) {
        return 0;
      }
      if (b.title.toLowerCase() > a.title.toLowerCase()) {
        return -1;
      }
      if (b.title.toLowerCase() === a.title.toLowerCase()) {
        return 0;
      }
      return 1;
    });
  }

  orderDateAscending(): void {
    this.caffs.sort((a: CaffItemDto, b: CaffItemDto) => {
      if (!b || !a || !a.lastModifiedAt || !b.lastModifiedAt) {
        return 0;
      }
      if (b.lastModifiedAt.toLowerCase() > a.lastModifiedAt.toLowerCase()) {
        return -1;
      }
      if (b.lastModifiedAt.toLowerCase() === a.lastModifiedAt.toLowerCase()) {
        return 0;
      }
      return 1;
    });
  }

  orderDateDescending(): void {
    this.caffs.sort((a: CaffItemDto, b: CaffItemDto) => {
      if (!b || !a || !a.lastModifiedAt || !b.lastModifiedAt) {
        return 0;
      }
      if (b.lastModifiedAt.toLowerCase() > a.lastModifiedAt.toLowerCase()) {
        return 1;
      }
      if (b.lastModifiedAt.toLowerCase() === a.lastModifiedAt.toLowerCase()) {
        return 0;
      }
      return -1;
    });
  }

  onFilterCaffs(): void {
    const dialogRef = this.filterDialog.open(FilterCaffsDialogComponent, {
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
