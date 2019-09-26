namespace SmeData.FTI.WebApi.Groupers
{
    using System.Collections.Generic;

    public class DocumentModel
    {
        public DocumentModel(DocumentTypeEnum docType, int id)
        {
            this.ConsolidatedActIds = new List<int>();
            this.DocType = docType;
            this.DocId = id;
        }

        public int DocId { get; set; }

        public DocumentTypeEnum DocType { get; set; }

        public List<int> ConsolidatedActIds { get; set; }

        public bool IsAdded { get; set; } = false;

        public override int GetHashCode()
        {
            return DocId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as DocumentModel;

            return DocId.Equals(other.DocId);
        }
    }
}
