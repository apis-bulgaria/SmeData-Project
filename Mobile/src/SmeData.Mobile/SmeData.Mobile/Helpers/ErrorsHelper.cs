using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmeData.Mobile.Helpers
{
    public static class ErrorsHelper
    {
        public static async Task DisplayError(IPageDialogService dialogService, Exception ex)
        {
            await dialogService.DisplayAlertAsync(Translator.GetString("Error"), ex.Message, Translator.GetString("Ok"));
        }
    }
}
