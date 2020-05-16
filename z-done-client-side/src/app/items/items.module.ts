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
import {MaterialAppsModule} from '../ngmaterial.module';
import {FormsModule} from '@angular/forms';
import {FolderComponent} from './pages/folder/folder.component';
import {FolderService} from './services/folder.service';
import {ListComponent} from './pages/list/list.component';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, MatDatepickerModule} from '@angular/material';
import {MAT_MOMENT_DATE_ADAPTER_OPTIONS, MatMomentDateModule, MomentDateAdapter} from '@angular/material-moment-adapter';
// tslint:disable-next-line:no-duplicate-imports
// tslint:disable-next-line:no-duplicate-imports
import * as _moment from 'moment';
import {defaultFormat as _rollupMoment} from 'moment';

const moment = _rollupMoment || _moment;

export const MY_FORMATS = {
  parse: {
    dateInput: 'LL',
  },
  display: {
    dateInput: 'LL',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};

@NgModule({
  declarations: [ItemListComponent, ItemDetailsComponent, MenuDetailsComponent, ItemsComponent, ItemComponent, MenuComponent, MainWindowComponent, FolderComponent, ListComponent],
  imports: [
    CommonModule,
    MaterialAppsModule,
    FormsModule,
    RouterModule,
    MatDatepickerModule,
    MatMomentDateModule

  ],
  providers: [DatePipe, ItemService, FolderService, {
    provide: DateAdapter,
    useClass: MomentDateAdapter,
    deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
  },

    {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS}]
})
export class ItemsModule {
}
