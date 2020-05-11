import {Project} from './project';

export class Folder {
  public id: number;
  public projectId: number;
  public name: string;

  public project: Project;
}
