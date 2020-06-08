import {Component, OnDestroy, OnInit} from '@angular/core';
import {ItemService} from '../../services/item.service';
import {Item} from '../../../models/item';
import {DetailService} from '../../services/detail.service';
import {Subscription} from 'rxjs';

@Component({
  selector: 'app-item-details',
  templateUrl: './item-details.component.html',
  styleUrls: ['./item-details.component.css']
})
export class ItemDetailsComponent implements OnInit, OnDestroy {

  activeItem: Item;
  itemDate: Date;
  subscriptionMain: Subscription;

  constructor(private itemService: ItemService, private detailService: DetailService) {
    this.activeItem = null;

    this.subscriptionMain =  this.itemService.reloadTaskSubject.subscribe(value => {
      this.itemService.getItem(value).subscribe(item => {
        this.activeItem = item;
        item.expiredDate == null ? this.itemDate = null : this.itemDate = new Date(item.expiredDate);
        this.detailService.activeItem = item;
        console.log('Loaded item: ', this.activeItem);
      });
      console.log('ID: ' + value);

    });
  }

  ngOnInit() {
  }

  ngOnDestroy(): void {
    this.subscriptionMain.unsubscribe();
  }

}
