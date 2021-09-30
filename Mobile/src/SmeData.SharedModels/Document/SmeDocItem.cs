using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SmeData.SharedModels.Document
{
    public enum SmeDocItemType { Title, Recital, Section, Chapter, Part, Article, Paragraph, SubParagraph, Point, Sentence, Letter, Number, Text, DocTitle, Preface };

    public class SmeDocItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([System.Runtime.CompilerServices.CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Id { get; set; } = string.Empty;
        public string FullId { get; set; } = string.Empty;
        public SmeDocItemType Type { get; set; } = SmeDocItemType.Text;
        public string Text { get; set; } = string.Empty;
        public string Heading { get; set; } = string.Empty;
        public string SubParagraph { get; set; } = string.Empty;
        public string SubHeading { get; set; } = string.Empty;
        public bool HasPal { get; set; } = false;
        public bool HasRecitals { get; set; } = false;
        public int TreeLevel { get; set; } = 0;
        public List<string> Recitals { get; set; } = new List<string>();
        public List<string> Articles { get; set; } = new List<string>();
        public List<string> OldArticles { get; set; } = new List<string>();
        public List<SmeDocItem> Childs { get; set; } = new List<SmeDocItem>();

        public bool IsBookmarkVisible
        {
            get
            {
                return (this.Type != SmeDocItemType.DocTitle) && (this.Type != SmeDocItemType.Recital);
            }
        }

        private bool isBookmarked = false;
        public bool IsBookmarked
        {
            get => isBookmarked;
            set
            {
                isBookmarked = value;
                RaisePropertyChanged();
            }
        }

        public SmeDocItem Clone()
        {
            return new SmeDocItem
            {
                Childs = this.Childs,
                HasPal = this.HasPal,
                HasRecitals = this.HasRecitals,
                Id = this.Id,
                FullId = this.FullId,
                Recitals = this.Recitals,
                Text = this.Text,
                Type = this.Type,
                Heading = this.Heading,
                SubHeading = this.SubHeading,
                SubParagraph = this.SubParagraph,
                TreeLevel = this.TreeLevel,
                IsBookmarked = this.IsBookmarked,
                Articles = this.Articles
            };
        }


    }
}
