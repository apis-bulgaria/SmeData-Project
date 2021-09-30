using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.SharedModels.Bookmark
{
    public class BookmarkModelList : List<BookmarkModel>
    {
        public string DocTitle { get; set; }
        public List<BookmarkModel> BookmarkModelItems => this;
    }
}
