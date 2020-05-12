import {Component, OnInit} from '@angular/core';
import {ItemService} from '../../services/item.service';
import {Item} from '../../../models/item';

@Component({
  selector: 'app-item-details',
  templateUrl: './item-details.component.html',
  styleUrls: ['./item-details.component.css']
})
export class ItemDetailsComponent implements OnInit {

  activeItem: Item;

  constructor(private itemService: ItemService) {
    this.activeItem = null;
    this.itemService.reloadTaskSubject.subscribe(value => {
      this.itemService.getItem(value).subscribe(item => {
        this.activeItem = item;
        console.log('Loaded item: ', this.activeItem);
      });
      console.log('ID: ' + value);

    });
  }

  ngOnInit() {
  }

}
