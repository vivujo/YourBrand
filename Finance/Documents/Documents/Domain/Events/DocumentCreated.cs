using YourBrand.Documents.Domain.Common;

namespace YourBrand.Documents.Domain.Events;

public class DocumentCreated : DomainEvent
{
    public DocumentCreated(string documentId)
    {
        DocumentId = documentId;
    }

    public string DocumentId { get; }
}
