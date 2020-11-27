import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import {
  CaffItemDetailsDto,
  CaffItemDto,
  CaffItemDtoPagedResponse,
  CaffItemService,
  FileDto,
  UserDto,
} from '../../../api/generated';
import { NewCaffDialogComponent } from '../new-caff-dialog/new-caff-dialog.component';
import { AuthService } from 'src/app/auth/auth.service';

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

  newCaffTitle: string;
  newCaffDesc: string;
  loggedInUserEmail: string;

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
        console.log(res);
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
}
