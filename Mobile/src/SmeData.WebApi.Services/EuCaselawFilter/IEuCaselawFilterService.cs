using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Services.EuCaselawFilter
{
    public interface IEuCaselawFilterService
    {
        int[] Filter(int[] ids);
        void Refresh();
    }
}
