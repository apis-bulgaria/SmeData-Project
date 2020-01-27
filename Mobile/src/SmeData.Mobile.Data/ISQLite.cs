using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.Mobile.Data
{
    public interface ISQLite
    {
        SQLiteAsyncConnection GetConnection(string dbPath);
    }
}
