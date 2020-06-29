export interface IUploadProps {
  onDrop(acceptedFiles: File[]): Promise<void>;
}
