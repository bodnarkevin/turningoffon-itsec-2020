import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { UserDto } from '../api/generated/model/userDto';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent implements OnInit {

    users: UserDto[] = [];

    constructor(private router: Router) { }

    ngOnInit() {
        this.getUsers();
    }

    // TODO: endpoint bekötése
    getUsers(): void {
        for(let i = 0; i < 80; i++) {
            this.users.push({
                id: i,
                email: 'test@test.hu',
                firstName: 'Test',
                lastName: 'User ' + i,
                fullName: 'Test User ' + i
            })
        }
    }

    // TODO: adott user profiljára navigálni
    onSelectUser(userId: number): void {
        this.router.navigate(['/profile'], {
            queryParams: {
                userId: userId
            }
        })
    }

}
