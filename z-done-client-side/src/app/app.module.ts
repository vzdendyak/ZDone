import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {AppComponent} from './app.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {MaterialAppsModule} from './ngmaterial.module';
import {DragDropModule} from '@angular/cdk/drag-drop';
import {MatCheckboxModule} from '@angular/material';
import {RouterModule} from '@angular/router';
import {ItemsModule} from './items/items.module';
import {MainWindowComponent} from './items/pages/main-window/main-window.component';
import {ItemService} from './items/services/item.service';
import {FolderService} from './items/services/folder.service';
import {CreateListFormComponent} from './items/components/create-list-form/create-list-form.component';
import {LoginModule} from './account/login.module';
import {LoginFunctionalityComponent} from './account/components/login-functionality/login-functionality.component';
import {RegistrationFunctionalityComponent} from './account/components/registration-functionality/registration-functionality.component';
import {AuthGuard} from './account/auth-guard';
import {AuthInterceptor} from './account/auth-interceptor';
import {CookieService} from 'ngx-cookie-service';

@NgModule({
  declarations: [
    AppComponent
  ],
  entryComponents: [CreateListFormComponent],
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
      {path: 'account/login', component: LoginFunctionalityComponent},
      {path: 'account/registration', component: RegistrationFunctionalityComponent},
      {path: 'windows/:folderId/:listId', component: MainWindowComponent, canActivate: [AuthGuard]},
      {path: 'windows', redirectTo: '/windows/0/all', canActivate: [AuthGuard]},
      {path: '', redirectTo: '/windows/0/all', pathMatch: 'full' }

    ]),
    ItemsModule,
    LoginModule
  ],
  providers: [ItemService, FolderService, {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true}, CookieService],
  bootstrap: [AppComponent]
})
export class AppModule {
}
