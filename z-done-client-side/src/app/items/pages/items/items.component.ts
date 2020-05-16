import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {Item} from '../../../models/item';
import {DatePipe} from '@angular/common';
import {ItemService} from '../../services/item.service';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})
export class ItemsComponent implements OnInit {
  @Input() items: Item[];
  @Input() name: string;
  @Output() reloadItem = new EventEmitter<number>();
  @Output() newItemName = new EventEmitter<string>();
  @Output() toDeleteItem = new EventEmitter<number>();
  itemName = '';

  constructor(public itemService: ItemService, private datepipe: DatePipe) {
  }

  ngOnInit() {
  }

  itemChanged(id: number) {
    this.reloadItem.emit(id);
    // console.log('ID: ' + id);
  }

  enterClicked() {
    this.newItemName.emit(this.itemName);
  }

  deleteClicked(id: number) {
    this.toDeleteItem.emit(id);
  }

  doneClicked(item: Item) {
    item.isDone = !item.isDone;
    this.itemService.insertNewItem(item);
    // this.reloadItem.emit(item.id);
  }
}
