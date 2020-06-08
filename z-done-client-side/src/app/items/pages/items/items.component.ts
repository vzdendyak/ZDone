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
  @Input() doneItems: Item[];
  @Input() unDoneItems: Item[];
  @Input() name: string;
  @Output() reloadItem = new EventEmitter<number>();
  @Output() newItemName = new EventEmitter<string>();
  @Output() toDeleteItem = new EventEmitter<number>();
  itemName = '';
  hideCompleted = false;

  constructor(public itemService: ItemService, public datePipe: DatePipe) {
  }

  ngOnInit() {
  }

  itemChanged(id: number) {
   // console.log('ITEMS');

    this.reloadItem.emit(id);


    // console.log('ID: ' + id);
  }

  enterClicked() {
    this.newItemName.emit(this.itemName);
    this.itemName = '';
  }

  deleteClicked(id: number) {
    this.toDeleteItem.emit(id);
  }

  doneClicked(item: Item) {
    item.isDone = !item.isDone;
    this.itemService.completeItem(item);
    // this.reloadItem.emit(item.id);
  }

  toogleCompleted() {
    this.hideCompleted = !this.hideCompleted;
  }
}
