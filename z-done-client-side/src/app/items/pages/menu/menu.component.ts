import {Component, Input, OnInit} from '@angular/core';
import {List} from '../../../models/list';
import {Folder} from '../../../models/folder';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  @Input() folders: Folder[];
  @Input() lists: List[];

  constructor() {
  }

  ngOnInit() {
  }

}
