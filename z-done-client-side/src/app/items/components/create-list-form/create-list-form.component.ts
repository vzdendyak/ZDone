import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from '@angular/forms';
import {MatDialogRef} from '@angular/material';
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
  selectedFolder: Folder;
  folders: Folder[];

  constructor(private fb: FormBuilder, public dialogRef: MatDialogRef<CreateListFormComponent>, public folderService: FolderService, private listService: ListService) {
    this.folderService.getFolders().subscribe(value => {
      this.folders = value;
      this.selectedFolder = null;
    });
  }

  ngOnInit() {
    this.initForm();
  }

  initForm() {
    this.myFirstReactiveForm = this.fb.group({
      name: [null, [Validators.required]],
      folder: [null]
    });
  }

  onSubmit() {
    const list: List = {
      id: 0,
      name: this.myFirstReactiveForm.controls.name.value,
      folderId: this.myFirstReactiveForm.controls.folder.value.id,
      folder: this.myFirstReactiveForm.controls.folder.value
    };

    console.log(list);
    this.listService.createList(list).subscribe(value => {
      this.dialogRef.close({folder: this.myFirstReactiveForm.controls.folder.value, list});
    });
    // this.folde
  }
}
