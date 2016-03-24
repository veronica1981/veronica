using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCCL_DAL
{
	public class FCCLDbContext : FCCLEntities
	{
		public FCCLApp Application { get; set; }

		public FCCLDbContext(FCCLApp application)
			: base()
		{
			Application = application;
		}
	}
}
