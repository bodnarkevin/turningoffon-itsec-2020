import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import {
  AddCommentDto,
  CaffAnimationDataDto,
  CaffDataDto,
  CaffItemDetailsDto,
  CaffItemService,
  CiffDataDto,
  CommentDto,
  CommentService,
  FileDto,
  UpdateCaffItemDto,
  UserDto,
} from '../api/generated';

@Component({
  selector: 'app-caff-details',
  templateUrl: './caff-details.component.html',
  styleUrls: ['./caff-details.component.css'],
})
export class CaffDetailsComponent implements OnInit {
  caffDataForm = new FormGroup({
    title: new FormControl('', Validators.required),
    description: new FormControl('', Validators.required),
    creator: new FormControl('', Validators.required),
    tags: new FormControl('', Validators.required),
    size: new FormControl('', Validators.required),
    creationdate: new FormControl('', Validators.required),
    uploaddate: new FormControl('', Validators.required),
  });
  newCommentForm = new FormGroup({
    commenttext: new FormControl('', Validators.required),
  });

  editingInProgress = false;
  queryParamSubscription: Subscription = Subscription.EMPTY;

  comments: CommentDto[] = [];

  // if there's a caffId, we're looking at the given caff's details
  caffId: number = null;

  testCaff: CaffItemDetailsDto = null;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private caffService: CaffItemService,
    private commentService: CommentService,
  ) {}

  ngOnInit(): void {
    this.queryParamSubscription = this.route.queryParams.subscribe(
      (params: Params) => {
        if (params && params.caffId) {
          this.caffId = params.caffId;
          // this.getCaffItemDetailsById();

          // testing
          // Todo: remove this when the page is ready
          const fileDto: FileDto = {
            id: 'testfileid',
            fileUri:
              'https://material.angular.io/assets/img/examples/shiba2.jpg',
          };
          const userDto: UserDto = {
            id: 11,
            email: 'hello@example.com',
            firstName: 'John',
            lastName: 'Doe',
            fullName: 'John Doe',
          };
          const ciffDataDto: CiffDataDto = {
            width: 200,
            height: 150,
            caption: 'This is a ciff caption',
            tags: ['#tag', '#tag2', '3tag'],
          };
          const animationsDto: Array<CaffAnimationDataDto> = [
            {
              order: 0,
              duration: 10,
              ciffData: ciffDataDto,
            },
          ];
          const caffDataDto: CaffDataDto = {
            creator: 'Creator Name',
            creation: '2020.01.02',
            animations: animationsDto,
          };
          this.testCaff = {
            id: this.caffId,
            title: 'this is a caff title',
            description: 'this is a caff description',
            downloadedTimes: 0,
            previewFile: fileDto,
            createdBy: userDto,
            caffData: caffDataDto,
          };
          this.caffDataForm.controls.title.setValue(this.testCaff.title);
          this.caffDataForm.controls.description.setValue(
            this.testCaff.description
          );
          this.caffDataForm.controls.creator.setValue(
            this.testCaff.createdBy.fullName
          );
          const tags: string[] = [];
          this.testCaff.caffData.animations.forEach((anim) => {
            anim.ciffData.tags.forEach((tag) => {
              tags.push(tag);
            });
          });
          this.caffDataForm.controls.tags.setValue(tags);
          this.caffDataForm.controls.size.setValue(
            this.testCaff.caffData.animations[0].ciffData.width.toString() +
              'x' +
              this.testCaff.caffData.animations[0].ciffData.height.toString()
          );
          this.caffDataForm.controls.creationdate.setValue(
            this.testCaff.caffData.creation
          );
          this.caffDataForm.controls.uploaddate.setValue(
            this.testCaff.caffData.creation
          );

          this.comments = [
            {
              id: 1,
              text: 'This is a new test comment. Hello bello hello.',
              createdAt: '2020.01.01',
              createdBy: userDto,
              lastModifiedAt: '2020.01.01',
            },
            {
              id: 2,
              text:
                'This is a new test comment. Hello bello hello. This is a new test comment. Hello bello hello. This is a new test comment. Hello bello hello. This is a new test comment. Hello bello hello.',
              createdAt: '2020.01.02',
              createdBy: userDto,
              lastModifiedAt: '2020.01.02',
            },
          ];
        }
      }
    );

    this.caffDataForm.disable();
  }

  /** Start caff data editing (enable form) */
  onEditCaffData(): void {
    this.editingInProgress = true;
    this.caffDataForm.enable();
    this.caffDataForm.controls.creator.disable();
    this.caffDataForm.controls.tags.disable();
    this.caffDataForm.controls.size.disable();
    this.caffDataForm.controls.creationdate.disable();
    this.caffDataForm.controls.uploaddate.disable();
  }

  /** Cancel caff data editing */
  onCancelEditing(): void {
    this.editingInProgress = false;
    this.caffDataForm.disable();
  }

  onSaveChanges(): void {
    const caffData: UpdateCaffItemDto = {
      title: this.caffDataForm.controls.fullName.value,
      description: this.caffDataForm.controls.fullName.value,
    };

    this.editingInProgress = false;
    this.caffDataForm.disable();
    if (this.caffId) {
      this.caffService.updateMyCaffItem(this.caffId, caffData).subscribe(
        (res: CaffItemDetailsDto) => {
          this.editingInProgress = false;
          this.caffDataForm.disable();
        },
        (err) => {
          alert('Caff data change failed');
        }
      );
    }
  }

  /** Get caff by ID */
  getCaffItemDetailsById(): void {
    this.caffService.getCaffItem(this.caffId).subscribe(
      (res: CaffItemDetailsDto) => {
        this.caffDataForm.controls.title.setValue(res.title);
        this.caffDataForm.controls.description.setValue(res.description);
      },
      (err) => {
        if (err.status === 404) {
          alert('CAFF not found.');
          this.router.navigate(['/list']);
        } else {
          // 401, 403, 500 if unauthorized, redirect to error
          this.router.navigate(['/error']);
        }
      }
    );
  }

  /* Get Comments */
  getComments(): void {
    this.caffService.getCaffItemComments(this.caffId).subscribe(
        (res: CommentDto[]) => {
            this.comments = res;
        },
        (err) => {
            alert('Something went wrong. Please try again later.');
        }
    );
  }

  /** Write a new comment */
  onSendComment(): void {
    const comment: AddCommentDto = {
      text: this.newCommentForm.controls.commentText.value,
    };

    if (this.caffId) {
      this.caffService.addCaffItemComment(this.caffId, comment).subscribe(
        (res: CommentDto) => {
          this.newCommentForm.controls.commentText.setValue('');
        },
        (err) => {
          alert('Adding new comment failed');
        }
      );
    }
  } // TODO: users can only delet their own comments

  /** Delete a comment */
  onDeleteComment(commentId: number): void {
    if (this.caffId) {
      this.commentService.deleteMyComment(commentId).subscribe(
        (res: CommentDto) => {
          alert('Successfully deleted comment');
        },
        (err) => {
          alert('Deleting comment failed');
        }
      );
    }
  }

  /** Delete caff file */
  onDeleteCaff(): void {
    if (this.caffId) {
      this.caffService.deleteMyCaffItem(this.caffId).subscribe(
        () => {
          alert('Successfully deleted caff');
        },
        (err) => {
          alert('Deleting caff failed');
        }
      );
    }
  }

  /** Download caff file */
  onDownloadCaff(): void {
    if (this.caffId) {
      this.caffService.downloadCaffItem(this.caffId).subscribe(
        (res: FileDto) => {
          alert('Successfully downloaded caff file');
        },
        (err) => {
          alert('Downloading caff failed');
        }
      );
    }
  }
}
