import {Component, Input, OnInit} from '@angular/core';
import {Item} from '../../../models/item';
import {DatePipe} from '@angular/common';
import {DetailService} from '../../services/detail.service';
import {ItemService} from '../../services/item.service';
import * as _moment from 'moment';
import {defaultFormat as _rollupMoment} from 'moment';
import {FormControl} from '@angular/forms';

const moment = _rollupMoment || _moment;


@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit {
  @Input() item: Item;
  @Input() date: Date;

  date2 = new FormControl(moment);

  constructor(public datePipe: DatePipe, public detailService: DetailService, private itemService: ItemService) {
    this.itemService.updatedTaskSubject.subscribe(value => {
      console.log('Updated_' + value);
    });

  }

  ngOnInit() {

  }

  reloadItem() {
    //console.log('newDate : ' + this.date.getDay());
    let date2 = new Date(this.date);
    console.log('newDate : ' + date2.toDateString());
    

    this.item.expiredDate = date2;
    this.itemService.insertNewItem(this.item);
  }

  doneButton() {
    //alert('DONE');
    this.item.isDone = !this.item.isDone;
    this.itemService.completeItem(this.item);
  }
}
