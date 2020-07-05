import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { List } from '../../../models/list';
import { Folder } from '../../../models/folder';
import { Route, Router } from '@angular/router';
import { IdentityService } from '../../../account/services/identity.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  @Input() folders: Folder[];
  @Input() lists: List[];
  @Output() addFolder = new EventEmitter<boolean>();
  userName = '';
  constructor(private router: Router, private identityService: IdentityService) {
    this.userName = localStorage.getItem('sub');
  }

  ngOnInit() {
  }

  goTo(folderId, listId) {
    this.router.navigateByUrl(`windows/${folderId}/${listId}`);
  }
  logout() {
    this.identityService.logOut().subscribe(value => {
      localStorage.clear();
      this.router.navigateByUrl('account/login');
    }, error => {
      console.log('Error logout: = ' + error);
    });
  }
  addFolderClick() {
      this.addFolder.emit(true);
  }
}
