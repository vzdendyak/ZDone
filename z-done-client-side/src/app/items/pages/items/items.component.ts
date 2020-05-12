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
  @Output() reloadItem = new EventEmitter<number>();

  constructor(public itemService: ItemService, private datepipe: DatePipe) {
  }

  ngOnInit() {
  }

  itemChanged(id: number) {
    this.reloadItem.emit(id);
    //console.log('ID: ' + id);
  }
}
