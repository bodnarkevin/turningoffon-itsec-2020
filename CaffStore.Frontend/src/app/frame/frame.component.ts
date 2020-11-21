import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Router } from '@angular/router';

@Component({
    selector: 'app-frame',
    templateUrl: './frame.component.html',
    styleUrls: ['./frame.component.css']
})
export class FrameComponent implements OnInit, OnChanges {

    @Input() title: string = '';

    menuOpened: boolean = false;

    constructor(private router: Router) { }

    ngOnInit() { }

    
    ngOnChanges(changes: SimpleChanges) {
        if (changes && changes.title && changes.title.currentValue !== changes.title.previousValue) {
            this.menuOpened  = false;
        }
    }

    onLogout(): void {
        // TODO: logout
        this.router.navigate(['/']);
    }

}
