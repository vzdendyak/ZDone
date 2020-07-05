import { Component, OnInit } from '@angular/core';
import { ListService } from '../../services/list.service';
import { FolderService } from '../../services/folder.service';
import { List } from '../../../models/list';
import { Folder } from '../../../models/folder';
import { MatDialog } from '@angular/material';
import { CreateListFormComponent } from '../create-list-form/create-list-form.component';
import { CreateFolderFormComponent } from '../create-folder-form/create-folder-form.component';

@Component({
  selector: 'app-menu-details',
  templateUrl: './menu-details.component.html',
  styleUrls: ['./menu-details.component.css'],
})
export class MenuDetailsComponent implements OnInit {
  folders: Folder[];
  // lists: List[];

  constructor(
    private listService: ListService,
    private folderService: FolderService,
    public dialog: MatDialog
  ) {
    this.folderService.getFolders().subscribe((value) => {
      this.folders = value;
    });

    this.folderService.openListDialogSubject.subscribe((value) => {
      this.openDialog(value);
    });
    this.folderService.openFolderDialogSubject.subscribe((value) => {
      this.openFolderDialog(value);
    });
    this.folderService.deleteListSubject.subscribe((value) => {
      this.deleteList(value);
    });
    this.folderService.deleteFolderSubject.subscribe(v => {
      this.deleteFolder(v);
    });
  }

  ngOnInit() { }

  openDialog(list: List) {
    let dialogRef;
    if (list == null) {
      dialogRef = this.dialog.open(CreateListFormComponent, {
        width: '450px',
      });
    } else {
      dialogRef = this.dialog.open(CreateListFormComponent, {
        width: '450px',
        data: {
          item: list,
        },
      });
    }

    dialogRef.afterClosed().subscribe((value) => {
      console.log('dialog closed');
      if (list == null) {
        if (value.folder != null) {
          const index = this.folders.findIndex(
            (fldr) => fldr.id == value.folder.id
          );
          this.folderService.folderIdToShow = value.folder.id;
          this.folders.splice(index, 1, value.folder);
        }
        // this.lists.push(value.list);
      } else {
        if (value.folder != null) {
          let index = this.folders.findIndex(
            (fldr) => fldr.id == value.folder.id
          );
          this.folders.splice(index, 1, value.folder);
          // index = this.lists.findIndex((l) => l.id == list.id);
          this.folderService.folderIdToShow = list.folderId;
          // this.lists.splice(index, 1, value.list);
        } else {
          // const index = this.lists.findIndex((l) => l.id === list.id);
          this.folderService.folderIdToShow = list.folderId;
          // this.lists.splice(index, 1, value.list);
        }
      }
    });
  }

  openFolderDialog(value: Folder) {
    let dialogRef;
    if (value) {
      dialogRef = this.dialog.open(CreateFolderFormComponent, {
        width: '450px',
        data: {
          title: 'Edit folder',
          folder: value
        }
      });
      dialogRef.afterClosed().subscribe(v => {
        if (!v.Folder) {
          return;
        }
        // const index = this.folders.findIndex(f => f.id == v.Folder.id);
        // this.folders[index].name = v.Folder.name;
      });
    } else {
      dialogRef = this.dialog.open(CreateFolderFormComponent, {
        width: '450px',
        data: {
          title: 'New folder'
        }
      });
      dialogRef.afterClosed().subscribe(v => {
        console.log('stop');
        if (!v.Folder) {
          return;
        }
        const index = this.folders.length;
        this.folders.splice(index, 1, v.Folder);
        this.folderService.folderIdToShow = v.Folder.id;
      });
    }


  }

  deleteFolder(value: Folder) {

    this.folderService.deleteFolder(value.id).subscribe(v => {
      console.log('Deleted');
      const index = this.folders.findIndex(f => f.id == value.id);
      this.folders.splice(index, 1);
    });

  }
  deleteList(list: List) {
    this.listService.deleteList(list.id).subscribe((value) => {
      let index = this.folders.findIndex((fldr) => fldr.id === list.folderId);
      this.folders.splice(index, 1, list.folder);
      // index = this.lists.findIndex((l) => l.id === list.id);
      // this.lists.splice(index, 1, list);
      this.folderService.folderIdToShow = list.folderId;
    });
  }
}
