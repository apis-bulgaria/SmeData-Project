using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.Data.Models
{
    public class BookmarksModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string DocIdentifier { get; set; }

        public string DocTitle { get; set; }

        [TextBlob("BookmarksParsAndTextBlobbed")]
        public Dictionary<string, string> BookmarksParsAndText { get; set; }
        public string BookmarksParsAndTextBlobbed { get; set; }
    }
}
