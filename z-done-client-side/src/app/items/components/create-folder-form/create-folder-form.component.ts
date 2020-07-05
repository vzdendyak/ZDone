import { Component, OnInit, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { Folder } from 'src/app/models/folder';
import { FolderService } from '../../services/folder.service';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-create-folder-form',
  templateUrl: './create-folder-form.component.html',
  styleUrls: ['../create-list-form/create-list-form.component.css']
})
export class CreateFolderFormComponent implements OnInit {
  reactiveForm: FormGroup;
  title: string;

  constructor(private fb: FormBuilder,
    public dialogRef: MatDialogRef<CreateFolderFormComponent>,
    @Inject(MAT_DIALOG_DATA) public data: FolderDialogData,
    private folderService: FolderService) {
    this.title = this.data.title;

  }

  ngOnInit() {
    this.initForm();
  }
  get getName() {
    return this.reactiveForm.get('Name');
  }

  initForm() {
    if (!this.data.folder) {
      this.reactiveForm = this.fb.group({
        Name: [null, [Validators.required]]
      });
    }
    else {
      this.reactiveForm = this.fb.group({
        Name: [this.data.folder.name, [Validators.required]]
      });
    }

  }

  onSubmit() {
    if (!this.data.folder) {
      let folder: Folder;
      folder = {
        name: this.reactiveForm.controls.Name.value,
        id: 0,
        isBasic: false,
        projectId: Number.parseInt(localStorage.getItem('projectId')),
        project: null
      };
      this.folderService.createFolder(folder).subscribe(value => {
        console.log('Created folder.');
        folder.id = value;
        this.dialogRef.close({ Folder: folder });
      }, error => {
        console.log('Error: ', (error as HttpErrorResponse).message);
      });
    } else {
      this.data.folder.name = this.reactiveForm.controls.Name.value;
      this.folderService.updateFolder(this.data.folder).subscribe(f => {
        console.log('Updated');
        this.dialogRef.close({ Folder: this.data.folder });
      });
    }
  }

  close() {
    this.dialogRef.close({ Folder: null });
  }
}
interface FolderDialogData {
  title: string;
  folder: Folder;
}
