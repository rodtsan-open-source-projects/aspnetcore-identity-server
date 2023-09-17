﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Clean.Architecture.SharedKernel;

// This can be modified to EntityBase<TId> to support multiple key types (e.g. Guid)
public abstract class EntityBase
{
	/// <summary>
	/// Gets or sets the of the primary key of the entity.
	/// </summary>
	  public int Id { get; set; }

	  private List<DomainEventBase> _domainEvents = new ();
	  [NotMapped]
	  public IEnumerable<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

	  protected void RegisterDomainEvent(DomainEventBase domainEvent) => _domainEvents.Add(domainEvent);
	  internal void ClearDomainEvents() => _domainEvents.Clear();

}
