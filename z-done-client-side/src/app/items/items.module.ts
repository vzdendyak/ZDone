import {NgModule} from '@angular/core';
import {CommonModule, DatePipe} from '@angular/common';
import {ItemListComponent} from './components/item-list/item-list.component';
import {ItemDetailsComponent} from './components/item-details/item-details.component';
import {MenuDetailsComponent} from './components/menu-details/menu-details.component';
import {ItemsComponent} from './pages/items/items.component';
import {ItemComponent} from './pages/item/item.component';
import {MenuComponent} from './pages/menu/menu.component';
import {MainWindowComponent} from './pages/main-window/main-window.component';
import {RouterModule} from '@angular/router';
import {ItemService} from './services/item.service';
import {AppModule} from '../app.module';
import {MaterialAppsModule} from '../ngmaterial.module';
import {FormsModule} from '@angular/forms';
import {FolderComponent} from './pages/folder/folder.component';
import {FolderService} from './services/folder.service';
import { ListComponent } from './pages/list/list.component';


@NgModule({
  declarations: [ItemListComponent, ItemDetailsComponent, MenuDetailsComponent, ItemsComponent, ItemComponent, MenuComponent, MainWindowComponent, FolderComponent, ListComponent],
  imports: [
    CommonModule,
    MaterialAppsModule,
    FormsModule,
  ],
  providers: [DatePipe, ItemService, FolderService]
})
export class ItemsModule { }
