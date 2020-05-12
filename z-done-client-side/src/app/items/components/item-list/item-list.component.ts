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
    this.itemService.getItems().subscribe(value => {
      this.items = value;
      console.log('GOT : ', this.items);

    });
  }

  reloadItem(id: number) {
    this.itemService.reloadTask(id);
    // console.log('ID: ' + id);

  }
}
