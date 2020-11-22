import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

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

    constructor(private router: Router, private adminUserService: AdminUserService) { }

    ngOnInit() {
        this.getUsers();
    }

    getUsers(): void {
        this.adminUserService.getPagedUsers(this.page, 10).subscribe(
            (res: UserDtoPagedResponse) => {
                if (this.page === 1) {
                    this.users = res.results;
                    this.pageCount = res.totalPageCount;
                } else {
                    this.users = [...this.users, ...res.results];
                } 
            },
            (err) => {
                alert('Something wen wrong. Please try again later.');
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
                userId: userId
            }
        })
    }

}
