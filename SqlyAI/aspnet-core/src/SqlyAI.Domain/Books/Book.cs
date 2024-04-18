using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace SqlyAI.Books;

public class Book : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }
}
