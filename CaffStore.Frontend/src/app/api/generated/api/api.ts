export * from './adminUser.service';
import { AdminUserService } from './adminUser.service';
export * from './caffItem.service';
import { CaffItemService } from './caffItem.service';
export * from './comment.service';
import { CommentService } from './comment.service';
export * from './user.service';
import { UserService } from './user.service';
export const APIS = [AdminUserService, CaffItemService, CommentService, UserService];
