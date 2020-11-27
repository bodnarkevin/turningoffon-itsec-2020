import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
    page = 1;
    /** Total page count */
    pageCount = 1;

    constructor(private router: Router, private adminUserService: AdminUserService, private _snackBar: MatSnackBar) { }

    ngOnInit(): void {
        this.getUsers();
    }

    getUsers(): void {
        // email: null, includeAdmins: false
        this.adminUserService.getPagedUsers(null, false, this.page, 10).subscribe(
            (res: UserDtoPagedResponse) => {
                if (this.page === 1) {
                    this.users = res.results;
                    this.pageCount = res.totalPageCount;
                } else {
                    this.users = [...this.users, ...res.results];
                }
            },
            (err) => {
                this._snackBar.open('Something went wrong. Please try again later.', null, {
                    duration: 2000,
                });
            }
        );
    }

    onLoadMore(): void {
        if (this.page !== this.pageCount) {
            this.page += 1;
            this.getUsers();
        }
    }

    onSelectUser(userId: number): void {
        this.router.navigate(['/profile'], {
            queryParams: {
                userId
            }
        });
    }
}
