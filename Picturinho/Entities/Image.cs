namespace Picturinho.Entities
{
    public class Image
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }

        public int AlbumId { get; set; }

        public Album Album { get; set; }
    }
}