using FCCL_DAL;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCCL_BL.Managers
{
	public class ReportManager
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private short application;
		private Repository<Report> repo;
		private FCCLDbContext context;
		
		public ReportManager(FCCLDbContext context)
		{
			repo = new Repository<Report>(context);
			application = (short)context.Application;
			this.context = context;
		}

		public Report GetByNumber(long reportNumber)
		{
			return repo.Get().FirstOrDefault(r => r.ReportNumber == reportNumber);
		}

		public Report GetByDateAndId(DateTime testDate, int objectId)
		{
			return repo.Get().FirstOrDefault(r => r.TestDate == testDate.Date && r.ObjectId == objectId);
		}

		public Report GetByDateAndName(DateTime testDate, string objectName)
		{
			return repo.Get().FirstOrDefault(r => r.TestDate == testDate.Date && r.ObjectName == objectName);
		}

		public List<Report> GetReports()
		{
			return repo.Get().ToList();
		}

		public void Save(Report report)
		{

			if(report.Id > 0)
			{
				report.Updated = DateTime.Now;
				repo.Update(report);
			}
			else
			{
				report.Created = DateTime.Now;
				report.Application = application;
				repo.Insert(report);
			}
		}

		public Report GetOrCreateReport(FCCLReportType type, int objectId, string objectName, DateTime testDate, DateTime printDate, long? reportNumber = null)
		{
			bool notCommited = true;
			int attemptCount = 0;
			Report report = null;

			while (notCommited && attemptCount < 5)
			{
				using (var tran = context.Database.BeginTransaction())
				{
					try
					{
						attemptCount++;
						logger.Info("GetOrCreateReport|objectName: {0}, reportNumber: {1}", objectName, reportNumber);
						if (reportNumber.HasValue)
						{
							report = GetByNumber(reportNumber.Value);
						}
						if (report == null)
						{
							report = repo.Create();
							if (reportNumber.HasValue)
							{
								report.ReportNumber = reportNumber.Value;
							}
							else
							{
								report.ReportNumber = repo.Get().OrderByDescending(r => r.Created).First().ReportNumber + 1;
								logger.Info("GetOrCreateReport|retrieved number: {0}", report.ReportNumber);
							}
						}
						else
						{
							logger.Info("GetOrCreateReport|retrieved report Id: {0}", report.Id);
						}
						report.ReportType = (short)type;
						report.ObjectId = objectId;
						report.ObjectName = objectName;
						report.TestDate = testDate.Year > 1753 ? testDate.Date : DateTime.Now.Date;
						report.PrintDate = printDate.Year > 1753 ? printDate.Date : DateTime.Now.Date;
						Save(report);
						context.SaveChanges();
						tran.Commit();
						notCommited = false;
					}
					catch (Exception ex)
					{
						logger.Fatal(string.Format("GetOrCreateReport|objectName: {0}, reportNumber: {1} at cnt {2}|msg:{3}|stack:{4}", objectName, reportNumber, attemptCount, ex.Message, ex.StackTrace));
					}
				}
			}
			if(notCommited)
			{
				throw new Exception("Nu am putut salva raportul");
			}
			logger.Info("GetOrCreateReport|objectName: {0}, reportNumber: {1}", objectName, reportNumber);
			return report;
		}

		public int GetReportsCount(DateTime? testDateStart, DateTime? testDateEnd, long? reportNoStart, long? reportNoEnd, string objectName)
		{
			return GetFilteredQuery(testDateStart, testDateEnd, reportNoStart, reportNoEnd, objectName).Count();
		}

		public List<Report> GetReportsPaged(DateTime? testDateStart, DateTime? testDateEnd, long? reportNoStart, long? reportNoEnd, string objectName, int startPage, int itemsPerPage)
		{
			return GetFilteredQuery(testDateStart, testDateEnd, reportNoStart, reportNoEnd, objectName).OrderByDescending(r => r.ReportNumber).Skip(startPage * itemsPerPage).Take(itemsPerPage).ToList();		
		}

		public List<Report> GetReportsFiltered(DateTime? testDateStart, DateTime? testDateEnd, long? reportNoStart, long? reportNoEnd, string objectName)
		{
			return GetFilteredQuery(testDateStart, testDateEnd, reportNoStart, reportNoEnd, objectName).OrderBy(r => r.ReportNumber).ToList(); 
		}

		private IEnumerable<Report> GetFilteredQuery(DateTime? testDateStart, DateTime? testDateEnd, long? reportNoStart, long? reportNoEnd, string objectName)
		{
			var query = repo.Get();
			if (testDateStart.HasValue)
			{
				query = query.Where(r => r.TestDate >= testDateStart.Value);
			}
			if (testDateEnd.HasValue)
			{
				query = query.Where(r => r.TestDate <= testDateEnd.Value);
			}
			if (reportNoStart.HasValue)
			{
				query = query.Where(r => r.ReportNumber >= reportNoStart.Value);
			}
			if (reportNoEnd.HasValue)
			{
				query = query.Where(r => r.ReportNumber <= reportNoEnd.Value);
			}
			if (!string.IsNullOrWhiteSpace(objectName))
			{
				query = query.Where(r => r.ObjectName.ToLower().Contains(objectName.ToLower()));
			}
			return query;
		}
	}
}
