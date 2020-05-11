import {Item} from './item';
import {User} from './user';

export class Comment {
  public id: number;
  public itemId: number;
  public userId: string;
  public text: string;
  public date: Date;

  public item: Item;
  public user: User;
}
