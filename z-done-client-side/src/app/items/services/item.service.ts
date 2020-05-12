import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable, Subject} from 'rxjs';
import {Item} from '../../models/item';

@Injectable({
  providedIn: 'root'
})
export class ItemService {
  url: string = environment.basicUrl + '/items';
  requestOptions: object = {
    headers: new HttpHeaders().append('Authorization', 'Bearer <yourtokenhere>'),
    responseType: 'text'
  };
  reloadTaskSubject = new Subject<number>();
  activeItemId: number;

  constructor(private http: HttpClient) {
    this.activeItemId = 0;
  }

  getItems(): Observable<Item[]> {
    return this.http.get<Item[]>(this.url);
  }

  getItem(id: number): Observable<Item> {
    return this.http.get<Item>(this.url + `/${id}`);
  }

  createItem(item: Item): Observable<Item> {
    return this.http.post<Item>(this.url, item, this.requestOptions);
  }

  updateItem(item: Item): Observable<Item> {
    console.log(item);
    return this.http.put<Item>(this.url, item, this.requestOptions);
  }

  deleteItem(itemId: number): Observable<Item> {
    return this.http.delete<Item>(this.url + `/${itemId}`, this.requestOptions);
  }

  reloadTask(id: number) {
    //console.log('ID: ' + id);
    this.activeItemId = id;
    this.reloadTaskSubject.next(id);
  }
}
