using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Services
{
    public interface IPathsProvider
    {
        string BasePath { get;}
        string PdfPath { get; }
    }
}
