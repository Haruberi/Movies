using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieAPI.Models
{
    public class MovieModel
    {
        public string Id { get; set; }
        public string MovieTitle { get; set; }
        public int ReleaseYear { get; set; }
        public string Description { get; set; }
    }
}
