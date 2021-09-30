using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SmeData.WebApi.Services.Documents
{
    public static class DbExtensions
    {
        public static T GetValue<T>(this DbDataReader dbReader, int ordinal)
        {
            var value = dbReader.GetValue(ordinal);
            if (value == DBNull.Value)
            {
                return (T)(object)null;
            }
            else
            {
                return (T)value;
            }
        }
    }
}
