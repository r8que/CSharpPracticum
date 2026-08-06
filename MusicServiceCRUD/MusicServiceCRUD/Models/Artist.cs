using System;
using System.Collections.Generic;
using System.Text;

namespace MusicServiceCRUD.Models
{
    public class Artist
    {
        public Guid ArtistId { get; set; }
        public string Name { get; set; } = null!;
        public string? Country { get; set; }
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
