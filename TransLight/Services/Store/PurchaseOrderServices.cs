using Microsoft.EntityFrameworkCore;
using TransLight.DataAccess.Common;
using TransLight.DataAccess.Data;
using TransLight.DataAccess.Filters.Store;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Store;
using TransLight.Services.Interfaces.Store;
using TransLight.Utility.Enums;

namespace TransLight.Services.Store
{
    public class PurchaseOrderServices(TransLightContext db) : BaseService<Transaction>(db), IPurchaseOrderService
    {
        private TransLightContext _db = db;

        public async Task<PaginatedResponse<PurchaseOrderVM>> GetPurchaseOrdersAsync(PurchaseOrderFilters filter, string? includeProperties = null)
        {
            IQueryable<Transaction> query = _db.Transactions;

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includProp in includeProperties
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includProp);
                }
            }

            query = query.Where(x => x.TransactionType == (int)TransactionType.PurchaseOrder);

            if (!string.IsNullOrWhiteSpace(filter.PoNo))
                query = query.Where(x => x.Id2Format != null && x.Id2Format.ToLower().Contains(filter.PoNo.ToLower()));

            if (!string.IsNullOrWhiteSpace(filter.PoDate))
                query = query.Where(x => x.Date == DateOnly.Parse(filter.PoDate));

            //if (!string.IsNullOrWhiteSpace(filter.PartyName))
            //    query = query.Where(x => x.Praties.Name != null && x.Parties.Name.ToLower().Contains(filter.Parties.Name.ToLower()));

            if (filter.Amount > 0)
                query = query.Where(x => x.TotalAmt == filter.Amount);

            if (filter.Cancel > -1)
                query = query.Where(x => x.Cancel == filter.Cancel);

            int totalPos = query.Count();

            var items = query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(x => new PurchaseOrderVM()
                {
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    CompanySiteId = x.CompanySiteId,
                    PartyId = x.PartyId,
                    PartySiteId = x.PartySiteId,
                    Id2Format = x.Id2Format,
                    Date = x.Date,
                    Type = x.Type,
                    TransactionType = x.TransactionType,
                    CurrencyId = x.CurrencyId,
                    ExchangeRate = x.ExchangeRate,
                    DeliveryType = (DeliveryTypes)x.DeliveryType,
                    Cancel = (YesNo)x.Cancel,
                    Remarks = x.Remarks,
                    BasicAmt = x.BasicAmt,
                    GstAmt = x.GstAmt,
                    RoundOffAmt = x.RoundOffAmt,
                    TotalAmt = x.TotalAmt,
                }).ToList();

            return new PaginatedResponse<PurchaseOrderVM>
            {
                Items = items,
                TotalItems = totalPos,
                TotalPages = (int)Math.Ceiling((double)totalPos / filter.PageSize),
                CurrentPage = filter.PageNumber
            };
        }

        public async Task<PurchaseOrderVM> GetForEditAsync(Guid? id)
        {
            if (id == null)
                return new PurchaseOrderVM();

            var poData = _db.Transactions
                .AsNoTracking()
                .Include(x => x.Company)
                .Include(x => x.CompanySite)
                .Include(x => x.Currency)
                .Where(x => x.Id == id).FirstOrDefault();

            if (poData == null)
                return new PurchaseOrderVM();

            var vm = new PurchaseOrderVM()
            {
                Id = poData.Id,
                CompanyId = poData.CompanyId,
                CompanySiteId = poData.CompanySiteId,
                PartyId = poData.PartyId,
                PartySiteId = poData.PartySiteId,
                Id2Format = poData.Id2Format,
                Date = poData.Date,
                Type = poData.Type,
                TransactionType = poData.TransactionType,
                CurrencyId = poData.CurrencyId,
                ExchangeRate = poData.ExchangeRate,
                DeliveryType = (DeliveryTypes)poData.DeliveryType,
                Cancel = (YesNo)poData.Cancel,
                Remarks = poData.Remarks,
                BasicAmt = poData.BasicAmt,
                GstAmt = poData.GstAmt,
                RoundOffAmt = poData.RoundOffAmt,
                TotalAmt = poData.TotalAmt,

                // PO Details List
            };

            return vm;
        }
    }
}
