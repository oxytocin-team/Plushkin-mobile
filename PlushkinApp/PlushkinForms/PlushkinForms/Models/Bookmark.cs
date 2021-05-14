using System;
using System.Collections.Generic;
using System.Text;

namespace PlushkinForms.Models
{
    class Bookmark
    {
        public int id { get; set; }
        public string type { get; set; }
        public string title { get; set; }

        public string siteName { get; set; }
        public string url { get; set; }
        public DateTime date { get; set; }
        public int user { get; set; }

        public override bool Equals(object obj)
        {
            Bookmark friend = obj as Bookmark;
            return id == friend.id;
        }

        public override int GetHashCode()
        {
            int hashCode = -778921025;
            hashCode = hashCode * -1521134295 + id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(type);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(url);
            hashCode = hashCode * -1521134295 + date.GetHashCode();
            hashCode = hashCode * -1521134295 + user.GetHashCode();
            return hashCode;
        }
    }
}
