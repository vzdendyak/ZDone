import {Component, Input, OnInit} from '@angular/core';
import {List} from '../../../models/list';
import {Folder} from '../../../models/folder';
import {Route, Router} from '@angular/router';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  @Input() folders: Folder[];
  @Input() lists: List[];

  constructor(private router: Router) {
  }

  ngOnInit() {
  }

  goTo(folderId, listId) {
    this.router.navigateByUrl(`windows/${folderId}/${listId}`);
  }
}
