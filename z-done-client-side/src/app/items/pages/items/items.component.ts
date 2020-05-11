import {Component, Input, OnInit} from '@angular/core';
import {Item} from '../../../models/item';

@Component({
  selector: 'app-items',
  templateUrl: './items.component.html',
  styleUrls: ['./items.component.css']
})
export class ItemsComponent implements OnInit {
  @Input() items: Item[];

  constructor() {
  }

  ngOnInit() {
  }

}
