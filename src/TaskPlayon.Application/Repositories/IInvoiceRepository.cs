using Microsoft.EntityFrameworkCore;
using TaskPlayon.Application.ViewModel;
using TaskPlayon.Core.Model;
using TaskPlayon.Infrastructure.DatabaseContext;

namespace TaskPlayon.Application.Repositories
{
    public interface IInvoiceRepository
    {
        Task<bool> CreateInvoiceWithCustomerAsync(InvoiceCreateVm vm);
        Task<List<InvoiceVm>> GetAllInvoicesAsync();
        Task<InvoiceCreateVm?> GetInvoiceByIdAsync(long id);
        Task<bool> UpdateInvoiceAsync(long invoiceId, InvoiceCreateVm vm);
    }

    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateInvoiceWithCustomerAsync(InvoiceCreateVm vm)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Save customer
                var customer = new Customer
                {
                    Name = vm.CustomerName,
                    Email = vm.CustomerEmail,
                    Phone = vm.CustomerPhone,
                    Address = vm.CustomerAddress
                };
                _context.Set<Customer>().Add(customer);
                await _context.SaveChangesAsync();

                // Save invoice
                var invoice = new Invoice
                {
                    CustomerId = customer.Id,
                    Date = vm.Date,
                    Total = vm.TotalAmount
                };
                _context.Set<Invoice>().Add(invoice);
                await _context.SaveChangesAsync();

                // Save invoice items
                var invoiceItems = vm.Items.Select(item => new InvoiceItem
                {
                    InvoiceId = invoice.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                }).ToList();
                _context.Set<InvoiceItem>().AddRange(invoiceItems);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }
        }

        public async Task<List<InvoiceVm>> GetAllInvoicesAsync()
        {
            return await _context.Set<Invoice>()
                .Include(i => i.Customer)
                .Include(i => i.InvoiceItem)
                    .ThenInclude(ii => ii.Product)
                .Select(i => new InvoiceVm
                {
                    Id = i.Id,
                    CustomerName = i.Customer.Name,
                    CustomerEmail = i.Customer.Email,
                    CustomerPhone = i.Customer.Phone,
                    CustomerAddress = i.Customer.Address,
                    Date = i.Date,
                    Total = i.Total,
                    Items = i.InvoiceItem.Select(ii => new InvoiceItemVmDisplay
                    {
                        ProductName = ii.Product.Name,
                        Quantity = ii.Quantity,
                        UnitPrice = ii.UnitPrice
                    }).ToList()
                })
                .ToListAsync();
        }

        public async Task<InvoiceCreateVm?> GetInvoiceByIdAsync(long id)
        {
            return await _context.Set<Invoice>()
        .Include(i => i.Customer)
        .Include(i => i.InvoiceItem)
            .ThenInclude(ii => ii.Product)
        .Where(i => i.Id == id)
        .Select(i => new InvoiceCreateVm
        {
            Id = i.Id,
            CustomerName = i.Customer.Name,
            CustomerPhone = i.Customer.Phone,
            CustomerEmail = i.Customer.Email,
            CustomerAddress = i.Customer.Address,
            Date = i.Date,
            TotalAmount = i.Total,
            Items = i.InvoiceItem.Select(ii => new InvoiceItemVm
            {
                ProductId = ii.ProductId,
                Quantity = ii.Quantity,
                UnitPrice = ii.UnitPrice
            }).ToList()
        })
        .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateInvoiceAsync(long invoiceId, InvoiceCreateVm vm)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var invoice = await _context.Set<Invoice>()
                    .Include(i => i.Customer)
                    .Include(i => i.InvoiceItem)
                    .FirstOrDefaultAsync(i => i.Id == invoiceId);

                if (invoice == null) return false;

                // Update customer
                invoice.Customer.Name = vm.CustomerName;
                invoice.Customer.Email = vm.CustomerEmail;
                invoice.Customer.Phone = vm.CustomerPhone;
                invoice.Customer.Address = vm.CustomerAddress;

                // Update invoice
                invoice.Date = vm.Date;
                invoice.Total = vm.Items.Sum(x => x.Quantity * x.UnitPrice);

                // Remove old items
                _context.Set<InvoiceItem>().RemoveRange(invoice.InvoiceItem);

                // Add updated items
                var items = vm.Items.Select(i => new InvoiceItem
                {
                    InvoiceId = invoice.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList();

                _context.Set<InvoiceItem>().AddRange(items);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }
    }
}
