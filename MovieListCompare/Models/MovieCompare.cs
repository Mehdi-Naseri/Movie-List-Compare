using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieListCompare.Models
{
    public class MovieCompare
    {
        public string Movie1 { get; set; }
        public string Movie2 { get; set; }
        public string Movie2Size { get; set; }
        public bool Folder { get; set; }
        public bool Copy { get; set; }
        public MovieCompare(string stringMovie1, string stringMovie2, string longMovie2Size, bool boolFolder, bool boolCopy)
        {
            this.Movie1 = stringMovie1;
            this.Movie2 = stringMovie2;
            this.Movie2Size = longMovie2Size;
            this.Folder = boolFolder;
            this.Copy = boolCopy;
        }
        public static string[] Properties = { "Movie1", "Movie2", "Movie2Size", "Folder", "Copy" };
        public List<string> PropertiesValues()
        {
            return new List<string> { Movie1, Movie2, Movie2Size.ToString(), Folder.ToString(), Copy.ToString() };
        }
    }
}
