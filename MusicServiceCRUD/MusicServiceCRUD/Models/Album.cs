using System;
using System.Collections.Generic;
using System.Text;

namespace MusicServiceCRUD.Models
{
    public class Album
    {
        public Guid AlbumId { get; set; }
        public string Title { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public decimal Price { get; set; }
        public Guid ArtistId { get; set; }
    }
}
