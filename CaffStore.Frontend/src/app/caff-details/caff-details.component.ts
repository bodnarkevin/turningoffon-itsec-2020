import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import {
  AddCommentDto,
  CaffItemDetailsDto,
  CaffItemService,
  CommentDto,
  CommentService,
  FileDto,
  UpdateCaffItemDto,
} from '../api/generated';
import { AuthService } from '../auth/auth.service';

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
    uploadername: new FormControl('', Validators.required),
    uploaderemail: new FormControl('', Validators.required),
  });
  newCommentForm = new FormGroup({
    commenttext: new FormControl('', Validators.required),
  });

  editingInProgress = false;
  queryParamSubscription: Subscription = Subscription.EMPTY;

  comments: CommentDto[] = [];

  // if there's a caffId, we're looking at the given caff's details
  caffId: number = null;

  caffDto: CaffItemDetailsDto = null;
  isAdmin = false;
  isOwnerOfCaff = false;
  userEmail: string;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private caffService: CaffItemService,
    private commentService: CommentService,
    private authService: AuthService,
    private _snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.queryParamSubscription = this.route.queryParams.subscribe(
      (params: Params) => {
        if (params && params.caffId) {
          this.caffId = params.caffId;
          this.getCaffItemDetailsById();
          this.getComments();
        }
      }
    );
    this.caffDataForm.disable();
    this.authService.isAdmin().then((res1) => {
      if (res1) {
        this.isAdmin = true;
      } else {
        this.isAdmin = false;
      }
    });
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
    this.caffDataForm.controls.uploaderemail.disable();
    this.caffDataForm.controls.uploadername.disable();
  }

  /** Cancel caff data editing */
  onCancelEditing(): void {
    this.editingInProgress = false;
    this.caffDataForm.disable();
  }

  onSaveChanges(): void {
    const caffData: UpdateCaffItemDto = {
      title: this.caffDataForm.controls.title.value,
      description: this.caffDataForm.controls.description.value,
    };

    this.editingInProgress = false;
    this.caffDataForm.disable();
    if (this.caffId) {
      this.caffService.updateMyCaffItem(this.caffId, caffData).subscribe(
        (res: CaffItemDetailsDto) => {
          this.editingInProgress = false;
          this.caffDataForm.disable();
          this._snackBar.open(
            'Successfully saved your changes!',
            null,
            {
              duration: 3000,
            }
          );
        },
        (err) => {
          this._snackBar.open(
            'Something went wrong during the update operation. Please try again later!',
            null,
            {
              duration: 3000,
            }
          );
        }
      );
    }
  }

  /** Get caff by ID */
  getCaffItemDetailsById(): void {
    this.caffService.getCaffItem(this.caffId).subscribe(
      (res: CaffItemDetailsDto) => {
        this.caffDto = res;
        this.caffDataForm.controls.title.setValue(res.title);
        this.caffDataForm.controls.description.setValue(res.description);
        this.setFormData();
        this.authService.getCurrentUserEmail().then((res2) => {
          if (res2) {
            // Check if the currently logged in user's email is equal to the creator's email of the viewed caff
            // to see if it's one of the caff files owned by the user.
            // Users can only edit/delete their own files.
            this.userEmail = res2;
            if (
              this.caffDto.createdBy != null &&
              this.caffDto.createdBy.email === res2
            ) {
              this.isOwnerOfCaff = true;
            }
          }
        });
      },
      (err) => {
        if (err.status === 404) {
          this.router.navigate(['/list']);
          this._snackBar.open('Caff file not found!', null, {
            duration: 2000,
          });
        } else if (err.status === 401 || err.status === 403) {
          this._snackBar.open(
            'You are not authorized to access this page!',
            null,
            {
              duration: 2000,
            }
          );
        } else {
          this._snackBar.open(
            'Something went wrong. Please try again later!',
            null,
            {
              duration: 2000,
            }
          );
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
        this._snackBar.open(
          'Something went wrong. Please try again later!',
          null,
          {
            duration: 3000,
          }
        );
      }
    );
  }

  /** Write a new comment */
  onSendComment(): void {
    const comment: AddCommentDto = {
      text: this.newCommentForm.controls.commenttext.value,
    };

    if (this.caffId) {
      this.caffService.addCaffItemComment(this.caffId, comment).subscribe(
        (res: CommentDto) => {
          this.newCommentForm.controls.commenttext.setValue(' ');
          this.getComments();
        },
        (err) => {
          this._snackBar.open(
            'Something went wrong during adding the new comment. Please try again later!',
            null,
            {
              duration: 3000,
            }
          );
        }
      );
    }
  }

  /** Delete a comment */
  onDeleteComment(commentId: number): void {
    if (this.caffId) {
      this.commentService.deleteMyComment(commentId).subscribe(
        (res: CommentDto) => {
          this.getComments();
        },
        (err) => {
          this._snackBar.open(
            'Something went wrong during the delete operation. Please try again later!',
            null,
            {
              duration: 3000,
            }
          );
        }
      );
    }
  }

  /** Delete caff file */
  onDeleteCaff(): void {
    if (this.caffId) {
      this.caffService.deleteMyCaffItem(this.caffId).subscribe(
        () => {
          this.router.navigate(['/list']);
        },
        (err) => {
          this._snackBar.open(
            'Something went wrong during the delete operation. Please try again later!',
            null,
            {
              duration: 3000,
            }
          );
        }
      );
    }
  }

  setFormData(): void {
    this.caffDataForm.controls.title.setValue(this.caffDto.title);
    this.caffDataForm.controls.description.setValue(this.caffDto.description);
    this.caffDataForm.controls.creator.setValue(this.caffDto.caffData.creator);
    const tags: string[] = [];
    this.caffDto.caffData.animations.forEach((anim) => {
      anim.ciffData.tags.forEach((tag) => {
        tags.push(tag);
      });
    });
    this.caffDataForm.controls.tags.setValue(tags);
    this.caffDataForm.controls.size.setValue(
      this.caffDto.caffData.animations[0].ciffData.width.toString() +
        'x' +
        this.caffDto.caffData.animations[0].ciffData.height.toString()
    );
    this.caffDataForm.controls.creationdate.setValue(
      this.caffDto.caffData.creation
    );
    this.caffDataForm.controls.uploaddate.setValue(this.caffDto.lastModifiedAt);

    // check if the created by field is null
    if (this.caffDto.createdBy == null) {
      this.caffDataForm.controls.uploaderemail.setValue('-');

      this.caffDataForm.controls.uploadername.setValue('-');
    } else {
      this.caffDataForm.controls.uploaderemail.setValue(
        this.caffDto.createdBy.email
      );

      this.caffDataForm.controls.uploadername.setValue(
        this.caffDto.createdBy.fullName
      );
    }
  }

  /** Download caff file */
  onDownloadCaff(): void {
    if (this.caffId) {
      this.caffService.downloadCaffItem(this.caffId).subscribe(
        (res: FileDto) => {
          this.downloadFile(res);
        },
        (err) => {
          this._snackBar.open(
            'Something went wrong during the download. Please try again later!',
            null,
            {
              duration: 2000,
            }
          );
        }
      );
    }
  }

  downloadFile(data: FileDto): void {
    window.open(data.fileUri);
  }
}
