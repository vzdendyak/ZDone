import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable} from 'rxjs';
import {Item} from '../../models/item';
import {Folder} from '../../models/folder';

@Injectable({
  providedIn: 'root'
})
export class FolderService {
  url: string = environment.basicUrl + '/items';
  requestOptions: object = {
    headers: new HttpHeaders().append('Authorization', 'Bearer <yourtokenhere>'),
    responseType: 'text'
  };

  constructor(private http: HttpClient) {
  }


  getFolders(): Observable<Folder[]> {
    return this.http.get<Folder[]>(this.url);
  }

  getFolder(id: number): Observable<Folder> {
    return this.http.get<Folder>(this.url + `/${id}`);
  }

  createFolder(folder: Item): Observable<Folder> {
    console.log('folder to create: ' + folder);
    return this.http.post<Folder>(this.url, folder);
  }

  updateFolder(folder: Item): Observable<Folder> {
    console.log(folder);
    return this.http.put<Folder>(this.url, folder, this.requestOptions);
  }

  deleteFolder(folderId: number): Observable<Folder> {
    return this.http.delete<Folder>(this.url + `/${folderId}`, this.requestOptions);
  }
}
