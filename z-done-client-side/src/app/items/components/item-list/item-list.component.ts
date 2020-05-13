import {Component, OnInit} from '@angular/core';
import {Item} from '../../../models/item';
import {ItemService} from '../../services/item.service';

@Component({
  selector: 'app-item-list',
  templateUrl: './item-list.component.html',
  styleUrls: ['./item-list.component.css']
})
export class ItemListComponent implements OnInit {
  items: Item[];

  constructor(private itemService: ItemService) {
  }

  ngOnInit() {
    this.getAllTasks();
  }

  getAllTasks() {
    this.itemService.getItems().subscribe(value => {
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
