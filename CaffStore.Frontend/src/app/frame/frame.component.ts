import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-frame',
    templateUrl: './frame.component.html',
    styleUrls: ['./frame.component.css']
})
export class FrameComponent implements OnInit {

    @Input() title: string = '';

    menuOpened: boolean = false;

    constructor(private router: Router) { }

    ngOnInit() { }

    onNavigateTo(navigateTo: string): void {
        this.router.navigate([navigateTo]);
        this.menuOpened = false;
    }

}
