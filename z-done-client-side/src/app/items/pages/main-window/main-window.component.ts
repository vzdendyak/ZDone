import {Component, OnInit} from '@angular/core';
import {ItemService} from '../../services/item.service';

@Component({
  selector: 'app-main-window',
  templateUrl: './main-window.component.html',
  styleUrls: ['./main-window.component.css']
})
export class MainWindowComponent implements OnInit {

  isCalendarView = false;

  constructor(private itemService: ItemService) {
    this.itemService.calendarSwitchSubject.subscribe(value => {
      this.isCalendarView = value;
    });
  }

  ngOnInit() {
  }

}
