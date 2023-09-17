using HotChocolate.Execution.Processing;
using HotChocolate.Execution;
using System.Transactions;

namespace AspNetCore.Identity.GraphQL.Scopes
{
	/// <summary>
	/// Represents the default mutation transaction scope handler implementation.
	/// </summary>
	public class DefaultTransactionScopeHandler : ITransactionScopeHandler
	{
		/// <summary>
		/// Creates a new transaction scope for the current
		/// request represented by the <see cref="IRequestContext"/>.
		/// </summary>
		/// <param name="context">
		/// The GraphQL request context.
		/// </param>
		/// <returns>
		/// Returns a new <see cref="ITransactionScope"/>.
		/// </returns>
		public virtual ITransactionScope Create(IRequestContext context)
		{
			return new DefaultTransactionScope(
				context,
				new TransactionScope(
				TransactionScopeOption.Required,
				new TransactionOptions
				{
					IsolationLevel = IsolationLevel.ReadCommitted
				}));
		}
	}
}
