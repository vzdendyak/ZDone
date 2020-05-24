import {Status} from './status';
import {List} from './list';

export class Item {
  public id: number;
  public listId: number;
  public parentId: number;
  public name: string;
  public description: string;
  public reason: string;
  public statusId: number;
  public isDone: boolean;

  public priority: number;
  public createdDate: Date;
  public expiredDate: Date;

  // nav
  public items?: Item[];
  public parent: Item;
  public status: Status;
  public list: List;


}
