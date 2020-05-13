import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Item} from '../../models/item';
import {Folder} from '../../models/folder';
import {List} from '../../models/list';

@Injectable({
  providedIn: 'root'
})
export class ListService {
  url: string = environment.basicUrl + '/lists';
  requestOptions: object = {
    headers: new HttpHeaders().append('Authorization', 'Bearer <yourtokenhere>'),
    responseType: 'text'
  };

  constructor(private http: HttpClient) {
  }


  getLists(): Observable<List[]> {
    return this.http.get<List[]>(this.url);
  }

  getList(id: number): Observable<List> {
    return this.http.get<List>(this.url + `/${id}`);
  }

  createList(list: List): Observable<number> {
    console.log('list to create: ' + list);
    return this.http.post<number>(this.url, list);
  }

  updateList(list: List): Observable<List> {
    console.log(list);
    return this.http.put<List>(this.url, list, this.requestOptions);
  }

  deleteList(listId: number): Observable<List> {
    return this.http.delete<List>(this.url + `/${listId}`, this.requestOptions);
  }
}
