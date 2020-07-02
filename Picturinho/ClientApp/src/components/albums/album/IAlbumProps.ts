import { AlbumModel } from '../../../models/album/AlbumModel';

export interface IAlbumProps {
  album: AlbumModel;
  handleRemoveAlbum(albumId: number): void;
}
