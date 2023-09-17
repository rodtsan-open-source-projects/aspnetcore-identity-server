using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Identity.Core.Responses
{
	public class CommandHandlerResponse
	{
		public CommandHandlerResponse() { }
		public CommandHandlerResponse(object id) : this(id, default) { }
		public CommandHandlerResponse(object id, string? message)
		{
			Id = id;
			Message = message;
		}

		public static CommandHandlerResponse Ok() => new();
		public static CommandHandlerResponse Ok(object id) => new(id);
		public static CommandHandlerResponse Ok(object id, string? message) => new(id, message);
		public virtual object? Id { get; set; } = default;
		public virtual string? Message { get; set; } = "Success";
		public virtual bool IsOk { get; set; } = true;
	}
}
