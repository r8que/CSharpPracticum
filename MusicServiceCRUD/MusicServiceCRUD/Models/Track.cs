using System;
using System.Collections.Generic;
using System.Text;

namespace MusicServiceCRUD.Models
{
    public class Track
    {
        public Guid TrackId { get; set; }
        public string Title { get; set; } = null!;
        public int DurationSeconds { get; set; }
        public Guid AlbumId { get; set; }
        public bool IsExplicit { get; set; }
    }
}
