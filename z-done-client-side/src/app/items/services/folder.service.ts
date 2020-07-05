import {Injectable} from '@angular/core';
import {environment} from '../../../environments/environment';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import {Observable, Subject} from 'rxjs';
import {Item} from '../../models/item';
import {Folder} from '../../models/folder';
import {List} from '../../models/list';

@Injectable({
  providedIn: 'root'
})
export class FolderService {
  url: string = environment.basicUrl + '/folders';
  requestOptions: object = {
    // headers: new HttpHeaders().append('Authorization', 'Bearer <yourtokenhere>'),
    // responseType: 'text'
    withCredentials: true

  };
  openListDialogSubject = new Subject<List>();
  openFolderDialogSubject = new Subject<Folder>();
  deleteFolderSubject = new Subject<Folder>();
  deleteListSubject = new Subject<List>();
  folderIdToShow: number = 0;

  constructor(private http: HttpClient) {
  }




  getFolders(): Observable<Folder[]> {
    return this.http.get<Folder[]>(this.url);
  }

  getRelatedLists(id: number): Observable<List[]> {
    return this.http.get<List[]>(`${this.url}/${id}/lists`);
  }

  getFolder(id: number): Observable<Folder> {
    return this.http.get<Folder>(this.url + `/${id}`);
  }

  createFolder(folder: Folder): Observable<number> {
    console.log('folder to create: ' + folder);
    return this.http.post<number>(this.url, folder);
  }

  updateFolder(folder: Folder): Observable<Folder> {
    console.log(folder);
    return this.http.put<Folder>(this.url, folder, this.requestOptions);
  }

  deleteFolder(folderId: number): Observable<Folder> {
    return this.http.delete<Folder>(this.url + `/${folderId}`, this.requestOptions);
  }
}
