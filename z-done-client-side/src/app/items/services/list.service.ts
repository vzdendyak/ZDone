import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Item} from '../../models/item';
import {List} from '../../models/list';

@Injectable({
  providedIn: 'root'
})
export class ListService {
  url: string = environment.basicUrl + '/lists';
  requestOptions: object = {
    // responseType: 'text',
    withCredentials: true

  };

  constructor(private http: HttpClient) {
  }


  getListItems(id: number): Observable<Item[]> {
    return this.http.get<Item[]>(`${this.url}/${id}/items`);
  }

  getDoneListItems(id: number): Observable<Item[]> {
    return this.http.get<Item[]>(`${this.url}/${id}/items/done`);
  }

  getUndoneListItems(id: number): Observable<Item[]> {
    return this.http.get<Item[]>(`${this.url}/${id}/items/undone`);
  }

  getLists(): Observable<List[]> {
    return this.http.get<List[]>(this.url);
  }

  getList(id: number): Observable<List> {
    return this.http.get<List>(this.url + `/${id}`);
  }

  createList(list: List): Observable<any> {
    const sendList: List = {
      id: 0,
      name: list.name,
      folderId: list.folder.id,
      folder: null,
      isBasic : false

    };
    return this.http.post(this.url, sendList, this.requestOptions);
  }

  updateList(list: List): Observable<List> {
    console.log(list);
    return this.http.put<List>(this.url, list, this.requestOptions);
  }

  deleteList(listId: number): Observable<List> {
    return this.http.delete<List>(this.url + `/${listId}`, this.requestOptions);
  }
}
