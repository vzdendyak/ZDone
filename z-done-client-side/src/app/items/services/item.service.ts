import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Observable, Subject} from 'rxjs';
import {Item} from '../../models/item';
import {formatDate} from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  url: string = environment.basicUrl + '/projects/items';
  itemUrl: string = environment.basicUrl + '/items';
  requestOptions: object = {
    // headers: new HttpHeaders().append('Authorization', 'Bearer <yourtokenhere>'),
    // responseType: 'text'
    withCredentials: true
  };
  reloadTaskSubject = new Subject<number>();
  insertTaskSubject = new Subject<Item>();
  calendarSwitchSubject = new Subject<boolean>();
  updatedTaskSubject = new Subject<boolean>();
  completeTaskSubject = new Subject<Item>();
  activeItemId: number;
  detailItem;

  constructor(private http: HttpClient) {
    this.activeItemId = 0;
  }

  getItems(): Observable<Item[]> {
    return this.http.get<Item[]>(this.url);
  }

  getItemsByDate(date: string): Observable<Item[]> {
    return this.http.get<Item[]>(`${this.url}/${date}`);
  }

  getCompletedItems(): Observable<Item[]> {
    return this.http.get<Item[]>(`${this.url}/completed`);
  }

  getDeletedItems(): Observable<Item[]> {
    return this.http.get<Item[]>(`${this.url}/deleted`);
  }

  getUnlistedItems(): Observable<Item[]> {
    return this.http.get<Item[]>(`${this.url}/unlisted`);
  }

  getItem(id: number): Observable<Item> {
    return this.http.get<Item>(this.itemUrl + `/${id}`);
  }

  createItem(item: Item): Observable<Item> {
    console.log('item to create: ' + item);
    return this.http.post<Item>(this.itemUrl, item);
  }

  updateItem(item: Item): Observable<Item> {
    console.log(item);
    return this.http.put<Item>(this.itemUrl, item, this.requestOptions);
  }

  deleteItem(itemId: number): Observable<Item> {
    return this.http.delete<Item>(this.itemUrl + `/${itemId}`, this.requestOptions);
  }

  reloadTask(id: number) {
    this.activeItemId = id;
    // console.log('SERVICE');
    this.reloadTaskSubject.next(id);
  }

  insertNewItem(item: Item) {
    this.insertTaskSubject.next(item);
    this.updateItem(item).subscribe(value => {
      this.updatedTaskSubject.next(true);
    });
  }

  completeItem(item: Item) {
    this.completeTaskSubject.next(item);
    this.updateItem(item).subscribe(value => {
      this.updatedTaskSubject.next(true);
    });
  }

  getNullItem() {
    const date = formatDate(new Date(), 'yyyy-MM-dd', 'en');
    const item: Item = {
      id: 0,
      name: ' ',
      parentId: null,
      statusId: 1,
      description: null,
      reason: null,
      createdDate: new Date(date),
      expiredDate: new Date(date),
      isDone: false,
      isDeleted: false,
      listId: 1,
      priority: null,
      parent: null,
      status: null,
      items: null,
      list: null
    };
    return item;
  }

  getDate(date: Date): string {
    const newDate = new Date(date);
    const month = environment.months[newDate.getMonth()];
    const text = `${newDate.getDate()} ${month}`;
    return text;
  }
  switchCalendar(state: boolean){
    this.calendarSwitchSubject.next(state);
  }
}
