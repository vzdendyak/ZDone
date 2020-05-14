import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {Item} from '../../models/item';
import {ItemService} from './item.service';

@Injectable({
  providedIn: 'root'
})
export class DetailService {
  activeItem: Item;

  constructor(private itemService: ItemService) {
    this.activeItem = itemService.getNullItem();
  }

  getItemDateMonth() {
    let itemDate = new Date(this.activeItem.expiredDate);
    const monthNumber = itemDate.getMonth();
    let month = environment.months[monthNumber];
    console.log('Month: ', monthNumber);
    return month;
  }
}
