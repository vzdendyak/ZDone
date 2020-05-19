import {Component, OnInit} from '@angular/core';
import {ListService} from '../../services/list.service';
import {FolderService} from '../../services/folder.service';
import {List} from '../../../models/list';
import {Folder} from '../../../models/folder';
import {MatDialog} from '@angular/material';
import {CreateListFormComponent} from '../create-list-form/create-list-form.component';

@Component({
  selector: 'app-menu-details',
  templateUrl: './menu-details.component.html',
  styleUrls: ['./menu-details.component.css']
})
export class MenuDetailsComponent implements OnInit {

  folders: Folder[];
  lists: List[];


  constructor(private listService: ListService, private folderService: FolderService, public dialog: MatDialog) {
    this.folderService.getFolders().subscribe(value => {
      this.folders = value;
    });
    this.listService.getLists().subscribe(value => {
      this.lists = value;
    });
    this.folderService.openDialogSubject.subscribe(value => {
      this.openDialog();
    });
  }

  ngOnInit() {
  }

  openDialog() {
    const dialogRef = this.dialog.open(CreateListFormComponent, {
      width: '450px'
    });
    dialogRef.afterClosed().subscribe(value => {
      if (value.folder != null){
        let index = this.folders.findIndex(fldr => fldr.id == value.folder.id);
        this.folders.splice(index, 1, value.folder);
      }
      this.lists.push(value.list);
    });
  }
}
