using Microsoft.EntityFrameworkCore;
using TransLight.DataAccess.Common;
using TransLight.DataAccess.Data;
using TransLight.DataAccess.Filters.Store;
using TransLight.DataAccess.Models;
using TransLight.DataAccess.ViewModels.Store;
using TransLight.Services.Common;
using TransLight.Services.Interfaces.Store;
using TransLight.Utility.Enums;

namespace TransLight.Services.Store
{
    public class PurchaseOrderServices(TransLightContext db, IHttpContextAccessor httpContextAccessor) : BaseService<Transaction>(db), IPurchaseOrderService
    {
        private TransLightContext _db = db;
        private IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<PaginatedResponse<PurchaseOrderVM>> GetPurchaseOrdersAsync(PurchaseOrderFilters filter, Guid companyId, string? includeProperties = null)
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

            query = query.Where(x => x.TransactionType == (int)TransactionType.PurchaseOrder && x.CompanyId == companyId);

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

                PurchaseOrderDetails = await _db.TransactionDetails.Where(x => x.TransactionId == poData.Id).Select(x => new PurchaseOrderDetailsVM
                {
                    Id = x.Id,
                    Vertical = x.Vertical,
                    TransactionId = x.TransactionId,
                    SrNo = x.SrNo,
                    ProductId = x.ProductId,
                    ProductName = x.Product.Name,
                    Description = x.Description,
                    Qty = x.Qty,
                    UnitId = x.UnitId,
                    UnitCode = x.Unit.Code,
                    Rate = x.Rate,
                    BasicAmt = x.BasicAmt,
                    GstPer = x.GstPer,
                    GstAmt = x.GstAmt,
                    TotalAmt = x.TotalAmt,
                }).ToListAsync()
            };

            return vm;
        }

        public async Task<ServiceReturn<Guid>> SaveAsync(PurchaseOrderVM vm)
        {
            await using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {

                Transaction? purchaseOrder;
                bool isNew = vm.Id == null;

                if (isNew)
                {
                    purchaseOrder = new Transaction
                    {
                        Id = Guid.Empty,
                        TransactionType = (int)TransactionType.PurchaseOrder
                    };

                    _db.Transactions.Add(purchaseOrder);
                }
                else
                {
                    purchaseOrder = (await _db.Transactions
                        .Include(x => x.Company)
                        .Include(x => x.CompanySite)
                        .Include(x => x.Currency)
                        .Include(x => x.TransactionDetails)
                        .Where(x => x.Id == vm.Id && x.TransactionType == (int)TransactionType.PurchaseOrder)
                        .FirstOrDefaultAsync());

                    if (purchaseOrder == null)
                    {
                        return new ServiceReturn<Guid>
                        {
                            Success = false,
                            Message = "Purchase Order not found."
                        };
                    }
                }
                purchaseOrder.Id2Format = vm.Id2Format;
                purchaseOrder.Date = vm.Date;
                purchaseOrder.CompanyId = vm.CompanyId;
                purchaseOrder.CompanySiteId = vm.CompanySiteId;
                purchaseOrder.PartyId = vm.PartyId;
                purchaseOrder.PartySiteId = vm.PartySiteId;
                purchaseOrder.CurrencyId = vm.CurrencyId;
                purchaseOrder.ExchangeRate = vm.ExchangeRate;
                purchaseOrder.DeliveryType = (int)vm.DeliveryType;
                purchaseOrder.Cancel = (int)vm.Cancel;
                purchaseOrder.BasicAmt = vm.BasicAmt;
                purchaseOrder.GstAmt = vm.GstAmt;
                purchaseOrder.RoundOffAmt = vm.RoundOffAmt;
                purchaseOrder.TotalAmt = vm.TotalAmt;
                purchaseOrder.Remarks = vm.Remarks ?? string.Empty;

                #region Sync PurchaseOrderDetails (update/insert/delete instead of wipe+reinsert)

                var incomingItems = vm.PurchaseOrderDetails
                    .Where(x => !x.IsSelected)
                    .ToList();

                // Existing rows keyed by Id for O(1) lookup
                var existingById = purchaseOrder.TransactionDetails.ToDictionary(x => x.Id);

                var keptIds = new HashSet<Guid>();
                int sr = 0;

                foreach (var item in incomingItems)
                {
                    sr += 1;
                    var basicAmt = item.Qty * item.Rate;
                    var gstAmt = Math.Round(basicAmt * item.GstPer / 100m, 2);
                    var totalAmt = basicAmt + gstAmt;

                    if (item.Id.HasValue && existingById.TryGetValue(item.Id.Value, out var existingDetail))
                    {
                        // update
                        existingDetail.SrNo = sr.ToString();
                        existingDetail.Vertical = "Store";
                        existingDetail.ProductId = item.ProductId;
                        existingDetail.Description = item.Description;
                        existingDetail.Qty = item.Qty;
                        existingDetail.UnitId = item.UnitId;
                        existingDetail.Rate = item.Rate;
                        existingDetail.BasicAmt = item.Qty * item.Rate;
                        existingDetail.GstPer = basicAmt;
                        existingDetail.GstAmt = gstAmt;
                        existingDetail.TotalAmt = totalAmt;

                        keptIds.Add(existingDetail.Id);
                    }
                    else
                    {
                        // insert
                        var newDetails = new TransactionDetail()
                        {
                            TransactionId = purchaseOrder.Id,
                            SrNo = sr.ToString(),
                            Vertical = "Store",
                            ProductId = item.ProductId,
                            Description = item.Description,
                            Qty = item.Qty,
                            UnitId = item.UnitId,
                            Rate = item.Rate,
                            BasicAmt = item.Qty * item.Rate,
                            GstPer = basicAmt,
                            GstAmt = gstAmt,
                            TotalAmt = totalAmt,
                        };

                        purchaseOrder.TransactionDetails.Add(newDetails);
                    }
                }

                // Remove rows that existed before but werent in the incoming list
                var detailsToRemove = purchaseOrder.TransactionDetails
                    .Where(x => x.Id != Guid.Empty && !keptIds.Contains(x.Id))
                    .ToList();

                if (detailsToRemove.Count > 0)
                    _db.TransactionDetails.RemoveRange(detailsToRemove);

                #endregion

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ServiceReturn<Guid>
                {
                    Success = true,
                    Message = vm.Id == null
                        ? "Purchase Order created successfully."
                        : "Purchase Order updated successfully.",
                    Data = purchaseOrder.Id
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
