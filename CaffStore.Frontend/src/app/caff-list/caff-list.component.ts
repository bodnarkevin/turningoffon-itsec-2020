import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CaffItemDto, CaffItemDtoPagedResponse, CaffItemService, FileDto, UserDto } from '../api/generated';

@Component({
  selector: 'app-caff-list',
  templateUrl: './caff-list.component.html',
  styleUrls: ['./caff-list.component.css']
})
export class CaffListComponent implements OnInit {
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
      email: "hello@example.com",
      firstName: "John",
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
  },
  { 
    id: 2,
    title: 'Egy masik kep',
    description: 'Ez egy sokkal rovidebb leiras.',
    downloadedTimes: 0,
    previewFile: this.fileDto,
    createdBy: this.userDto,
},
{ 
  id: 3,
  title: 'Egy harmadik',
  description: 'Egy hosszu leiras...... Nagyon hosszu leiras meg mindig tgart ez a leiras es meg mindig tart mikor lesz mar vege uristen de hosszu jajj jajjajajaj. Na csak kinyogted! Be volt kapcsolva ujujujujujujuj',
  downloadedTimes: 0,
  previewFile: this.fileDto3,
  createdBy: this.userDto,
},
{ 
  id: 4,
  title: 'Egy negyediiiik',
  description: 'kaki leiras',
  downloadedTimes: 0,
  previewFile: this.fileDto3,
  createdBy: this.userDto,
},
{ 
  id: 5,
  title: 'Egy otodik',
  description: 'leiras',
  downloadedTimes: 0,
  previewFile: this.fileDto,
  createdBy: this.userDto,
}];

  /** Current page */
  page: number = 1;
  /** Total page count */
  pageCount: number = 1;

  constructor(private router: Router, private caffItemService: CaffItemService) { }

  ngOnInit(): void {
    // TODO: test with server on
    // this.getCaffItems();
  }

  onViewDetails(caffId: number): void {
    //alert('You have clicked the card.' + caffId);
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

  /*
  onSelectCaff(caffId: number): void {
      this.router.navigate(['/caff'], {
          queryParams: {
              userId: caffId
          }
      });
  }
  */
}
