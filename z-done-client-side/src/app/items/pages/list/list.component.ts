import {Component, Input, OnInit} from '@angular/core';
import {List} from '../../../models/list';
import {Router} from '@angular/router';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ListComponent implements OnInit {

  @Input() list: List;

  constructor(private router: Router) { }

  ngOnInit() {

  }

  goTo(folderId, listId) {
    this.router.navigateByUrl(`windows/${folderId}/${listId}`);
  }
}
