using FCCL_DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCCL_BL.Managers
{
	public class IntervalManager
	{
		private short application;
		private CachedRepository<Interval> repo;

		public IntervalManager(FCCLDbContext context)
		{
			repo = new CachedRepository<Interval>(context);
			application = (short)context.Application;
		}

		public Interval GetIntervalByType(FCCLTestType testType)
		{
			return repo.Get().Where(s => (s.Application == application || s.Application == 0)).OrderByDescending(s => s.Application)
				.FirstOrDefault(s => s.Type == (int)testType);
		}

		public bool IsWithinLimits(FCCLTestType testType, string value)
		{
			bool isWithin = true;
			decimal convertedValue;
			Interval interval = GetIntervalByType(testType);
			if(decimal.TryParse(value, out convertedValue) && convertedValue > 0)
			{
				if(interval.MinVal > convertedValue || convertedValue > interval.MaxVal)
				{
					isWithin = false;
				}
			}
			return  isWithin;
		}
	}
}
