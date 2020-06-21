import {Component, OnInit, ViewChild} from '@angular/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import {FullCalendarComponent} from '@fullcalendar/angular';
import {ItemService} from '../../services/item.service';
import {Item} from '../../../models/item';
import {formatDate} from '@angular/common';

@Component({
  selector: 'app-item-calendar',
  templateUrl: './item-calendar.component.html',
  styleUrls: ['./item-calendar.component.css']
})
export class ItemCalendarComponent implements OnInit {
  @ViewChild('calendar', null) calendarComponent: FullCalendarComponent;
  calendarEvents = [
  ];
  calendarPlugins = [dayGridPlugin, timeGridPlugin, interactionPlugin];
  items: Item[];


  constructor(private itemService: ItemService) {
    this.itemService.getItems().subscribe(value => {
      this.items = value.filter(i => i.isDeleted == false);
      // this.calendarEvents = this.items.map(item => ({title: item.name, id: item.id, date: item.expiredDate, allDay: true}));
      this.calendarEvents = this.items.map(this.decomposite);
    });
  }

  decomposite(item: Item) {
    if (item.expiredDate == null) {
      const date = formatDate(new Date(), 'yyyy-MM-dd', 'en');
      return {title: item.name, id: item.id, date: new Date(date), allDay: true};
    }
    return {title: item.name, id: item.id, date: item.expiredDate, allDay: true};
  }

  handleDateClick(arg) {
    console.log('date');
    console.log(arg);
  }

  handleEventClick(arg) {
    this.itemService.reloadTask(arg.event.id);
    console.log('event');
    console.log(arg);
  }

  handleEventDragStop(arg) {
    console.log('drag stop');
    console.log(arg);
  }

  addEvent() {
    const calApi = this.calendarComponent.getApi();
    // this.calendarEvents = this.calendarEvents.concat({title: 'HELLO', allDay: true, date: '2020-06-21'});
    // this.calendarEvents.push({title: 'HELLO', allDay: true, date: '2020-06-21'});
  }

  ngOnInit() {

  }

}
