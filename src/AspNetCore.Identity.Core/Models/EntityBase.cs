using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore.Identity.Core.Models;

public class EntityBase
{
	public DateTime CreatedDate { get; set; }
	public Guid? CreatedById { get; set; }
	public User? CreatedBy { get; set; }
	public DateTime? LastEditedDate { get; set; }
	public Guid? LastEditedById { get; set; }
	public User? LastEditedBy { get; set; }
	public bool Deleted { get; set; }
	public bool Disabled { get; set; }

}
