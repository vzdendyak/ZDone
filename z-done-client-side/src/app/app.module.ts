import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {AppComponent} from './app.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {HttpClientModule} from '@angular/common/http';
import {MaterialAppsModule} from './ngmaterial.module';
import {DragDropModule} from '@angular/cdk/drag-drop';
import {MatCheckboxModule} from '@angular/material';
import {RouterModule} from '@angular/router';
import {ItemsModule} from './items/items.module';
import {MainWindowComponent} from './items/pages/main-window/main-window.component';
import {ItemService} from './items/services/item.service';
import {FolderService} from './items/services/folder.service';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    MaterialAppsModule,
    DragDropModule,
    FormsModule,
    MatCheckboxModule,
    ReactiveFormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    RouterModule.forRoot([
      {path: 'windows', component: MainWindowComponent}
    ]),
    ItemsModule
  ],
  providers: [ItemService, FolderService],
  bootstrap: [AppComponent]
})
export class AppModule {
}
