import {Component, Input, OnInit} from '@angular/core';
import {Item} from '../../../models/item';
import {DatePipe} from '@angular/common';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.css']
})
export class ItemComponent implements OnInit {
  @Input() item: Item;

  constructor(public datePipe: DatePipe) {
  }

  ngOnInit() {
  }

}
