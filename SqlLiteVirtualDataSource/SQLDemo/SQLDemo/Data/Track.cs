using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLDemo.Data
{
    [Table("tracks")]
    public class Track
    {
        [PrimaryKey, AutoIncrement]
        public int TrackId { get; set; }

        [MaxLength(200)]
        public string Name { get; set; }

        public int AlbumId { get; set; }

        [Column("Title")]
        public string AlbumTitle { get; set; }

        public int MediaTypeId { get; set; }

        public int GenreId { get; set; }

        [MaxLength(220)]
        public string Composer { get; set; }

        public int Milliseconds { get; set; }

        public int Bytes { get; set; }

        public decimal UnitPrice { get; set; }
    }
}
