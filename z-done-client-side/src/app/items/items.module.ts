import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ItemListComponent } from './components/item-list/item-list.component';
import { ItemDetailsComponent } from './components/item-details/item-details.component';
import { MenuDetailsComponent } from './components/menu-details/menu-details.component';
import { ItemsComponent } from './pages/items/items.component';
import { ItemComponent } from './pages/item/item.component';
import { MenuComponent } from './pages/menu/menu.component';
import { MainWindowComponent } from './pages/main-window/main-window.component';
import {RouterModule} from '@angular/router';



@NgModule({
  declarations: [ItemListComponent, ItemDetailsComponent, MenuDetailsComponent, ItemsComponent, ItemComponent, MenuComponent, MainWindowComponent],
  imports: [
    CommonModule
  ]
})
export class ItemsModule { }
