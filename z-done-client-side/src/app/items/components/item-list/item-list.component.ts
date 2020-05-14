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
  items: Item[];
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
            this.currentList = null;
            this.items = null;
            break;

          case 'week':
            this.listName = 'Week';
            this.currentList = null;
            this.items = null;
            break;

          case 'completed':
            this.listName = 'Completed';
            this.currentList = null;
            this.items = null;
            break;

          case 'trash':
            this.listName = 'Trash';
            this.currentList = null;
            this.items = null;
            break;

          case 'calendar':
            this.listName = 'Calendar';
            this.currentList = null;
            this.items = null;
            break;

          case 'inbox':
            this.listName = 'Inbox';
            this.currentList = null;
            this.items = null;
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
        console.log('Handled listId: ' + value.listId);
      }

    });
  }

  ngOnInit() {
  }

  getAllTasks() {
    this.itemService.getItems().subscribe(value => {
      this.items = value;
    });
  }

  getTaskByListId(id: number) {
    this.listService.getListItems(id).subscribe(value => {
      this.items = value;
      console.log('GOT : ', this.items);
    });
  }

  reloadItem(id: number) {
    this.itemService.reloadTask(id);
  }

  createItem(name: string) {
    console.log('Got: ' + name);
    const item = this.itemService.getNullItem();
    this.items[0] === null ? item.listId = null : item.listId = this.items[0].listId;
    item.name = name;
    this.itemService.createItem(item).subscribe(value => {
      console.log('Created:');
      this.items.push(value);
    });
  }

  deleteItem(id: number) {
    this.itemService.deleteItem(id).subscribe(value => {
      console.log('Deleted: ' + id);
      let index = this.items.findIndex(i => i.id === id);
      this.items.splice(index, 1);
    });
  }

}
