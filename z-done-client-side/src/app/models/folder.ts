import {Project} from './project';

export class Folder {
  public id: number;
  public projectId: number;
  public name: string;
  public isBasic: boolean;
  public project: Project;
}
