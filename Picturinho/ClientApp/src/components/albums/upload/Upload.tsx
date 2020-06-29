import React from 'react';
import { useDropzone } from 'react-dropzone';

import { IUploadProps } from './IUploadProps';
import styles from './Upload.module.scss';

export const Upload: React.FC<IUploadProps> = ({
  onDrop,
}: IUploadProps): JSX.Element => {
  const { getRootProps, getInputProps } = useDropzone({
    onDrop,
  });

  return (
    <section className="container">
      <div {...getRootProps({ className: styles.dropzone })}>
        <input {...getInputProps()} />
        <p>Drag and drop image(s) here, or click to select some</p>
      </div>
    </section>
  );
};
