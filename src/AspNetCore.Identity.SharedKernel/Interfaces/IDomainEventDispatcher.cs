
namespace Clean.Architecture.SharedKernel.Interfaces;

public interface IDomainEventDispatcher<TKey>
{
  Task DispatchAndClearEvents(IEnumerable<EntityBase> entitiesWithEvents);
}
