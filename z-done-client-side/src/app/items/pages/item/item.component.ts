import {Component, Input, OnInit} from '@angular/core';
import {Item} from '../../../models/item';
import {DatePipe} from '@angular/common';
import {DetailService} from '../../services/detail.service';
import {ItemService} from '../../services/item.service';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit {
  @Input() item: Item;
  @Input() date: Date;

  constructor(public datePipe: DatePipe, public detailService: DetailService, private itemService: ItemService) {
    this.itemService.updatedTaskSubject.subscribe(value => {
      alert('Updated_'+value);
    });
  }

  ngOnInit() {

  }

  reloadItem() {
    console.log('S');
    this.itemService.insertNewItem(this.item);
  }

}
