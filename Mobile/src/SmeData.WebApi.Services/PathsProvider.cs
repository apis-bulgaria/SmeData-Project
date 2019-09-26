using System;
using System.Collections.Generic;
using System.Text;

namespace SmeData.WebApi.Services
{
    public class PathsProvider: IPathsProvider
    {
        public string BasePath { get; set; }
        public string PdfPath { get; set; }
    }
}
