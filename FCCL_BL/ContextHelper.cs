using FCCL_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCCL_BL
{
	public class ContextHelper
	{
		public static FCCLDbContext GetContext(string app)
		{
			FCCLDbContext context = null;
			FCCLApp appNo;
			if(Enum.TryParse(app, out appNo))
			{
				context = new FCCLDbContext(appNo);
			}
			if(context == null)
			{
				throw new Exception("Could not create FCCLDbContext!");
			}
			return context;
		}
	}
}
