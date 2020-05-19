import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {List} from '../../../models/list';
import {Router} from '@angular/router';
import {MatMenuTrigger} from '@angular/material';
import {FolderService} from '../../services/folder.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  @Input() list: List;
  @ViewChild(MatMenuTrigger, {static: false}) trigger: MatMenuTrigger;

  contextMenuPosition = {x: '0px', y: '0px'};

  constructor(private router: Router, public folderService: FolderService) {
  }

  ngOnInit() {

  }

  goTo(folderId, listId) {
    this.router.navigateByUrl(`windows/${folderId}/${listId}`);
  }

  rightClickList(event) {
    event.preventDefault();
    this.contextMenuPosition.x = event.clientX + 'px';
    this.contextMenuPosition.y = event.clientY + 'px';
    this.trigger.menu.focusFirstItem('mouse');
    this.trigger.openMenu();
  }

  editListClick() {
    this.folderService.openListDialogSubject.next(this.list);
  }

  deleteListClick() {
    this.folderService.deleteListSubject.next(this.list);
  }
}
