import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormControl, FormGroup } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';

import { AdminUserService, UserDtoPagedResponse } from '../api/generated';
import { UserDto } from '../api/generated/model/userDto';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

    users: UserDto[] = [];
    /** Current page */
    page: number = 1;
    /** Total page count */
    pageCount: number = 1;

    searchForm = new FormGroup({
        email: new FormControl(''),
        showAdmins: new FormControl(false)
    });

    constructor(private router: Router, private adminUserService: AdminUserService, private _snackBar: MatSnackBar) { }

    ngOnInit(): void {
        this.getUsers();
    }

    /** Get user list */
    getUsers(): void {
        let email = null;
        if (this.searchForm.controls.email.value.length > 0) {
            email = this.searchForm.controls.email.value;
        }

        this.adminUserService.getPagedUsers(email, this.searchForm.controls.showAdmins.value, this.page, 10).subscribe(
            (res: UserDtoPagedResponse) => {
                if (this.page === 1) {
                    this.users = res.results;
                    this.pageCount = res.totalPageCount;
                } else {
                    this.users = [...this.users, ...res.results];
                }
            },
            (err) => {
                if (err.status === 401 || err.status === 403) {
                    this._snackBar.open('You are not authorized to access this page!', null, {
                        duration: 2000,
                    });
                } else {
                    this._snackBar.open('Something went wrong. Please try again later.', null, {
                        duration: 2000,
                    });
                }
            }
        );
    }

    /** Pagination */
    onLoadMore(): void {
        if (this.page !== this.pageCount) {
            this.page += 1;
            this.getUsers();
        }
    }

    /** On user selection */
    onSelectUser(userId: number): void {
        this.router.navigate(['/profile'], {
            queryParams: {
                userId
            }
        });
    }

    /** On filter */
    onFilter(): void {
        this.page = 1;
        this.getUsers();
    }
}
