import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';

import { CaffItemDetailsDto, CaffItemDto, CaffItemDtoPagedResponse, CaffItemService, FileDto, UserDto } from '../../../api/generated';
import { NewCaffDialogComponent } from '../new-caff-dialog/new-caff-dialog.component';

@Component({
    selector: 'app-shared-caff-list',
    templateUrl: './shared-caff-list.component.html',
    styleUrls: ['./shared-caff-list.component.css']
})
export class SharedCaffListComponent implements OnInit {

    // list, own
    @Input() type: string;

    fileDto: FileDto = {
        id: 'testfileid',
        fileUri: 'https://material.angular.io/assets/img/examples/shiba2.jpg',
    };
    fileDto3: FileDto = {
        id: 'testfileid3',
        fileUri: 'https://material.angular.io/assets/img/examples/shiba2.jpg',
    };
    userDto: UserDto = {
        id: 11,
        email: 'hello@example.com',
        firstName: 'John',
        lastName: 'Doe',
        fullName: 'John Doe'
    };
    caffs: CaffItemDto[] = [{
        id: 1,
        title: 'Hello test title',
        description: 'Ez egy nagyon szep kep, es ez a leirasa, ami szinten nagyon szep.',
        downloadedTimes: 0,
        previewFile: this.fileDto,
        createdBy: this.userDto,
        createdAt: '2020.11.24',
        lastModifiedAt: '2020.11.24'
    },
    {
        id: 2,
        title: 'Egy masik kep',
        description: 'Ez egy sokkal rovidebb leiras.',
        downloadedTimes: 0,
        previewFile: this.fileDto,
        createdBy: this.userDto,
        createdAt: '2020.11.24',
        lastModifiedAt: '2020.11.24'
    },
    {
        id: 3,
        title: 'Egy harmadik',
        description: 'Egy hosszu leiras...... Nagyon hosszu leiras meg mindig tgart ez a leiras es meg mindig tart mikor lesz mar vege uristen de hosszu jajj jajjajajaj. Na csak kinyogted! Be volt kapcsolva ujujujujujujuj',
        downloadedTimes: 0,
        previewFile: this.fileDto3,
        createdBy: this.userDto,
        createdAt: '2020.11.24',
        lastModifiedAt: '2020.11.24'
    },
    {
        id: 4,
        title: 'Egy negyediiiik',
        description: 'kaki leiras',
        downloadedTimes: 0,
        previewFile: this.fileDto3,
        createdBy: this.userDto,
        createdAt: '2020.11.24',
        lastModifiedAt: '2020.11.24'
    },
    {
        id: 5,
        title: 'Egy otodik',
        description: 'leiras',
        downloadedTimes: 0,
        previewFile: this.fileDto,
        createdBy: this.userDto,
        createdAt: '2020.11.24',
        lastModifiedAt: '2020.11.24'
    }];

    /** Current page */
    page = 1;
    /** Total page count */
    pageCount = 1;

    newCaffTitle: string;
    newCaffDesc: string;

    constructor(private router: Router, private caffItemService: CaffItemService, public dialog: MatDialog) { }

    ngOnInit() {
        // TODO: test with server on
        // this.getCaffItems();
        console.log('page type onInit: ' + this.type);
    }

    onViewDetails(caffId: number): void {
        this.router.navigate(['/caff'], {
            queryParams: {
                caffId
            }
        });
    }

    getCaffItems(): void {
        this.caffItemService.getPagedCaffItems(this.page, 10).subscribe(
            (res: CaffItemDtoPagedResponse) => {
                if (this.page === 1) {
                    this.caffs = res.results;
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
            data: { title: this.newCaffTitle, descripton: this.newCaffDesc }
        });

        dialogRef.afterClosed().subscribe(result => {
            console.log('The dialog was closed');
            if (result && result.controls) {
                const title = result.controls.title.value;
                const description = result.controls.description.value;
                console.log(title + '    ' + description);
                console.log(result.controls.cafffile.value as Blob);

                // todo

                this.caffItemService.addCaffItem(title, description, result.controls.cafffile.value as Blob).subscribe(
                    (res: CaffItemDetailsDto) => {
                        console.log(res);
                        // todo: navigate to new caff details???
                        /*
                        this.router.navigate(['/caff'], {
                          queryParams: {
                              caffId: res.id
                          }
                      });
                      */
                    },
                    (err) => {
                        alert('Caff data upload failed');
                    }
                );
            }
        });
    }

}

