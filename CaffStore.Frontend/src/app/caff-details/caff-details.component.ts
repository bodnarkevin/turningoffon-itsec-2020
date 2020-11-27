import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
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
    private authService: AuthService
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
            if (this.caffDto.createdBy != null && this.caffDto.createdBy.email === res2) {
              this.isOwnerOfCaff = true;
            }
          }
        });
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
      text: this.newCommentForm.controls.commenttext.value,
    };

    if (this.caffId) {
      this.caffService.addCaffItemComment(this.caffId, comment).subscribe(
        (res: CommentDto) => {
          this.newCommentForm.controls.commenttext.setValue(null);
          this.getComments();
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
          this.getComments();
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

  setFormData(): void {
    console.log(this.caffDto);
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
    this.caffDataForm.controls.uploaddate.setValue(
      this.caffDto.caffData.creation
    );

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
          alert('Successfully downloaded caff file');
        },
        (err) => {
          alert('Downloading caff failed');
        }
      );
    }
  }
}
