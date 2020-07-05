import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material';
import {Folder} from '../../../models/folder';
import {FolderService} from '../../services/folder.service';
import {List} from '../../../models/list';
import {ListService} from '../../services/list.service';

@Component({
  selector: 'app-create-list-form',
  templateUrl: './create-list-form.component.html',
  styleUrls: ['./create-list-form.component.css']
})
export class CreateListFormComponent implements OnInit {

  myFirstReactiveForm: FormGroup;
  selectedFolder: Folder = null;
  folders: Folder[];
  compareFn: ((f1: any, f2: any) => boolean) | null = this.compareByValue;

  constructor(private fb: FormBuilder, public dialogRef: MatDialogRef<CreateListFormComponent>,
              public folderService: FolderService,
              private listService: ListService,
              @Inject(MAT_DIALOG_DATA) public data: DialogData) {
    this.folderService.getFolders().subscribe(value => {
      this.folders = value;
      // this.myFirstReactiveForm.setValue({name: this.data.item.name, folder: this.data.item.folder});
    });
  }

  ngOnInit() {
    this.initForm();
    // this.set();
  }

  compareByValue(f1: any, f2: any) {
     return f1 && f2 && f1.name === f2.name;
  }

  get getFolder(){
      return this.myFirstReactiveForm.get('folder');
  }

  get getName(){
    return this.myFirstReactiveForm.get('Name');
  }
  set() {
    setTimeout(() => {
      this.myFirstReactiveForm.patchValue({Name: 'stop', folder: this.data.item.folder});
    }, 0);

  }

  initForm() {
    if (this.data != null) {
      this.selectedFolder = this.data.item.folder;
      this.myFirstReactiveForm = this.fb.group({
        Name: [this.data.item.name, [Validators.required]],
        folder: [this.selectedFolder, [Validators.required]]
      });
      console.log('log1');

    } else {
      this.myFirstReactiveForm = this.fb.group({
        Name: [null, [Validators.required]],
        folder: [null, [Validators.required]]
      });
    }
  }

  close() {
    this.dialogRef.close({folder: null, list: null});
  }

  onSubmit() {
    let list: List;
    if (this.data != null) {
      list = {
        id: this.data.item.id,
        name: this.myFirstReactiveForm.controls.Name.value,
        folderId: this.myFirstReactiveForm.controls.folder.value.id,
        folder: this.myFirstReactiveForm.controls.folder.value,
        isBasic : false
      };
      this.listService.updateList(list).subscribe(value => {
        this.dialogRef.close({folder: this.myFirstReactiveForm.controls.folder.value, list});
      });
    } else {
      console.log('logging');

      list = {
        id: 0,
        name: this.myFirstReactiveForm.controls.Name.value,
        folderId: this.myFirstReactiveForm.controls.folder.value.id,
        folder: this.myFirstReactiveForm.controls.folder.value,
        isBasic : false
      };
      this.listService.createList(list).subscribe(value => {
        this.dialogRef.close({folder: this.myFirstReactiveForm.controls.folder.value, list});
      });
    }
  }
}

export interface DialogData {
  item: List;
}
