import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
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

  constructor(private http: HttpClient) {
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
}
