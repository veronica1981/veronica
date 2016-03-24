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
	public class OrderManager
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		private static Logger loggerReceptie = LogManager.GetLogger("Receptie");
		private short application;
		private Repository<Order> repo;
		private FCCLDbContext context;

		public OrderManager(FCCLDbContext context)
		{
			repo = new Repository<Order>(context);
			application = (short)context.Application;
			this.context = context;
		}

		public Order GetByNumber(long orderNumber)
		{
			return repo.Get().FirstOrDefault(r => r.OrderNumber == orderNumber);
		}

		public Order GetByNumberAndImported(long orderNumber)
		{
			return repo.Get().FirstOrDefault(r => r.OrderNumber == orderNumber && !r.Imported);
		}

		public List<Order> GetOrders()
		{
			return repo.Get().ToList();
		}

		public void Save(Order order)
		{

			if (order.Id > 0)
			{
				order.Updated = DateTime.Now;
				repo.Update(order);
			}
			else
			{
				order.Created = DateTime.Now;
				order.Application = application;
				repo.Insert(order);
			}
		}

		public void Save(List<Order> orders)
		{
			List<long> orderNumbers = orders.Select(o => o.OrderNumber).ToList();
			List<Order> existingOrders = repo.Get().Where(o => orderNumbers.Contains(o.OrderNumber) && !o.Imported).ToList();
			
			foreach(var eOrder in existingOrders)
			{
				var newOrder = orders.First(o => o.OrderNumber == eOrder.OrderNumber && o.ClientName == eOrder.ClientName);
				if (newOrder != null)
				{
					loggerReceptie.Info("Am mai gasit comanda numarul{0} din data {1} pentru clientul {1} si am adaugat {2} mostre la cele {3} existente.", newOrder.OrderNumber, newOrder.ReceivedDate, newOrder.SampleCount, eOrder.SampleCount);
					orders.Remove(newOrder);
					eOrder.AnalyzedSampleCount += newOrder.AnalyzedSampleCount;
					eOrder.SampleCount += newOrder.SampleCount;
					eOrder.Imported = newOrder.Imported;

					orders.Add(eOrder);
				}
			}

			foreach(var order in orders)
			{
				Save(order);
			}
		}

		public Order GetOrCreateOrder(string fullOrderNumber, string clientName, DateTime sampleDate, DateTime receiveDate, bool imported, int? sampleCount, int? clientId, long? orderNumber = null)
		{
			logger.Info("GetOrCreateOrder|clientName: {0}, orderNumber: {1}", clientName, orderNumber);
			Order order = null;
			if (orderNumber.HasValue)
			{
				order = GetByNumberAndImported(orderNumber.Value);
			}
			if (order == null)
			{
				order = repo.Create();
				if (orderNumber.HasValue)
				{
					order.OrderNumber = orderNumber.Value;
				}
				else
				{
					order.OrderNumber = repo.Get().Max(r => r.OrderNumber) + 1;
					logger.Info("GetOrCreateOrder|retrieved number: {0}", order.OrderNumber);
				}
			}
			else
			{
				logger.Info("GetOrCreateOrder|retrieved order Id: {0}", order.Id);
			}
			order.FullOrderNumber = fullOrderNumber;
			order.ClientId = clientId.HasValue ? clientId.Value : -1;
			order.ClientName = clientName;
			order.SampleDate = sampleDate;
			order.ReceivedDate = receiveDate;
			order.Imported = imported;
			order.SampleCount = sampleCount;
			Save(order);
			context.SaveChanges();
			logger.Info("GetOrCreateOrder|clientName: {0}, reportNumber: {1}", clientName, orderNumber);
			return order;
		}

		public int GetOrdersCount(DateTime? receivedDateStart, DateTime? receivedDateEnd, long? orderNoStart, long? orderNoEnd, string clientName)
		{
			return GetFilteredQuery(receivedDateStart, receivedDateEnd, orderNoStart, orderNoEnd, clientName).Count();
		}

		public List<Order> GetOrdersPaged(DateTime? receivedDateStart, DateTime? receivedDateEnd, long? orderNoStart, long? orderNoEnd, string clientName
			, int startPage, int itemsPerPage)
		{
			return GetFilteredQuery(receivedDateStart, receivedDateEnd, orderNoStart, orderNoEnd, clientName).OrderByDescending(r => r.OrderNumber).Skip(startPage * itemsPerPage).Take(itemsPerPage).ToList();
		}

		public List<Order> GetOrdersFiltered(DateTime? receivedDateStart, DateTime? receivedDateEnd, long? orderNoStart, long? orderNoEnd, string clientName)
		{
			return GetFilteredQuery(receivedDateStart, receivedDateEnd, orderNoStart, orderNoEnd, clientName).OrderBy(r => r.OrderNumber).ToList();
		}

		private IEnumerable<Order> GetFilteredQuery(DateTime? receivedDateStart, DateTime? receivedDateEnd, long? orderNoStart, long? orderNoEnd, string clientName)
		{
			var query = repo.Get();
			if (receivedDateStart.HasValue)
			{
				query = query.Where(r => r.ReceivedDate >= receivedDateStart.Value);
			}
			if (receivedDateEnd.HasValue)
			{
				query = query.Where(r => r.ReceivedDate <= receivedDateEnd.Value);
			}
			if (orderNoStart.HasValue)
			{
				query = query.Where(r => r.OrderNumber >= orderNoStart.Value);
			}
			if (orderNoEnd.HasValue)
			{
				query = query.Where(r => r.OrderNumber <= orderNoEnd.Value);
			}
			if (!string.IsNullOrWhiteSpace(clientName))
			{
				query = query.Where(r => r.ClientName.ToLower().Contains(clientName.ToLower()));
			}
			return query;
		}

	}
}
