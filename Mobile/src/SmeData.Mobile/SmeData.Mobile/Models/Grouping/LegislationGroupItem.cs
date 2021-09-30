using SmeData.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace SmeData.Mobile.Models.Grouping
{
    public class LegislationGroupItem : ObservableCollection<DocumentResponseModel>
    {
        public string Heading { get; private set; }
        public LegislationGroupItem(CategoryResponseModel model)
        {
            this.Heading = model.Heading;
            foreach (var item in model.Items)
            {
                this.Add(item);
            }
        }
    }
}
