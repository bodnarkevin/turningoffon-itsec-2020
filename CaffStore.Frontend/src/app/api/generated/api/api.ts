export * from './adminCaffItem.service';
import { AdminCaffItemService } from './adminCaffItem.service';
export * from './adminComment.service';
import { AdminCommentService } from './adminComment.service';
export * from './adminUser.service';
import { AdminUserService } from './adminUser.service';
export * from './caffItem.service';
import { CaffItemService } from './caffItem.service';
export * from './comment.service';
import { CommentService } from './comment.service';
export * from './user.service';
import { UserService } from './user.service';
export const APIS = [AdminCaffItemService, AdminCommentService, AdminUserService, CaffItemService, CommentService, UserService];
