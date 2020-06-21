import {Component, OnInit} from '@angular/core';
import {Item} from '../../../models/item';
import {ItemService} from '../../services/item.service';
import {ActivatedRoute} from '@angular/router';
import {ListService} from '../../services/list.service';
import {List} from '../../../models/list';

@Component({
  selector: 'app-item-list',
  templateUrl: './item-list.component.html',
  styleUrls: ['./item-list.component.css']
})
export class ItemListComponent implements OnInit {
  doneItems: Item[];
  unDoneItems: Item[];
  listName = '';
  currentList: List;

  constructor(private itemService: ItemService, public route: ActivatedRoute, private listService: ListService) {
    this.route.params.subscribe(value => {
      if (value.folderId) {
        console.log('Handled folderId: ' + value.folderId);
      }
      if (value.listId) {
        let listId = value.listId;
        listId = listId.toString().toLowerCase();
        switch (listId) {
          case 'all':
            this.listName = 'All';
            this.currentList = null;
            this.getAllTasks();
            break;
          case 'today':
            this.listName = 'Today';
            this.itemService.getItemsByDate('today').subscribe(items => {
              this.doneItems = items.filter(i => i.isDone == true);
              this.unDoneItems = items.filter(i => i.isDone == false);
            });
            this.currentList = null;
            break;

          case 'week':
            this.listName = 'Week';
            this.currentList = null;
            this.itemService.getItemsByDate('week').subscribe(items => {
              this.doneItems = items.filter(i => i.isDone == true);
              this.unDoneItems = items.filter(i => i.isDone == false);
            });
            break;

          case 'completed':
            this.listName = 'Completed';
            this.currentList = null;
            this.getCompleted();
            this.unDoneItems = null;
            break;

          case 'trash':
            this.listName = 'Trash';
            this.currentList = null;
            this.getDeletedTasks();
            break;

          case 'calendar':
            this.listName = 'Calendar';
            this.itemService.switchCalendar(true);
            return;
            this.currentList = null;
            this.doneItems = null;
            this.unDoneItems = null;
            break;

          case 'inbox':
            this.listName = 'Inbox';
            this.currentList = null;
            this.itemService.getUnlistedItems().subscribe(items => {
              this.doneItems = items.filter(i => i.isDone == true);
              this.unDoneItems = items.filter(i => i.isDone == false);
            });
            break;

          default:
            this.listService.getList(value.listId).subscribe(list => {
              if (list.folderId == value.folderId) {
                this.listName = list.name;
                this.currentList = list;
                this.getTaskByListId(value.listId);
              }
            });
            break;
        }
        this.itemService.switchCalendar(false);
        console.log('Handled listId: ' + value.listId);
      }

    });
    this.itemService.insertTaskSubject.subscribe(value => {
      let index;
      if (value.isDone) {
        index = this.doneItems.findIndex(i => i.id == value.id);
        this.doneItems[index] = value;
      } else {
        index = this.unDoneItems.findIndex(i => i.id == value.id);
        this.unDoneItems[index] = value;
      }

    });

    this.itemService.completeTaskSubject.subscribe(value => {
      if (value.isDone) {
        let index = this.unDoneItems.findIndex(i => i.id == value.id);
        this.doneItems.push(value);
        this.unDoneItems.splice(index, 1);
      } else {
        let index = this.doneItems.findIndex(i => i.id == value.id);
        this.unDoneItems.push(value);
        this.doneItems.splice(index, 1);
      }
    });
  }

  ngOnInit() {
  }

  getAllTasks() {
    this.itemService.getItems().subscribe(items => {
      this.doneItems = items.filter(i => i.isDone == true && i.isDeleted == false);
      this.unDoneItems = items.filter(i => i.isDone == false && i.isDeleted == false);
    });
  }

  getCompleted() {
    this.itemService.getCompletedItems().subscribe(items => {
      this.doneItems = items;
    });
  }

  getTaskByListId(id: number) {
    this.listService.getDoneListItems(id).subscribe(value => {
      this.doneItems = value;
      console.log('GOT : ', this.doneItems);
    });
    this.listService.getUndoneListItems(id).subscribe(value => {
      this.unDoneItems = value;
      console.log('GOT : ', this.doneItems);
    });
  }

  reloadItem(id: number) {
    //console.log('ITEM_LIST');
    this.itemService.reloadTask(id);
  }

  createItem(name: string) {
    console.log('Got: ' + name);
    const item = this.itemService.getNullItem();
    this.unDoneItems[0] === null ? item.listId = null : item.listId = this.doneItems[0].listId;
    item.name = name;
    this.itemService.createItem(item).subscribe(value => {
      console.log('Created:');
      this.unDoneItems.push(value);
    });
  }

  deleteItem(id: number) {
    this.itemService.deleteItem(id).subscribe(value => {
      console.log('Deleted: ' + id);
      let index = this.doneItems.findIndex(i => i.id === id);
      if (index === -1) {
        index = this.unDoneItems.findIndex(i => i.id === id);
        this.unDoneItems.splice(index, 1);
        return;
      }
      this.doneItems.splice(index, 1);
    });
  }

  getDeletedTasks() {
    this.itemService.getDeletedItems().subscribe(items => {
      this.doneItems = items.filter(i => i.isDone == true);
      this.unDoneItems = items.filter(i => i.isDone == false);
    });
  }


}
