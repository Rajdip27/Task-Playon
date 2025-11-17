using AutoMapper;
using TaskPlayon.Application.Repositories.Base;
using TaskPlayon.Application.ViewModel;
using TaskPlayon.Core.Model;
using TaskPlayon.Infrastructure.DatabaseContext;
using TaskPlayon.Infrastructure.Healper.Acls;

namespace TaskPlayon.Application.Repositories;

public interface IProductRepository:IBaseRepository<Product,ProductVm,long>
{
}

public class ProductRepository(IMapper mapper, ApplicationDbContext context, DapperApplicationDbContext dapperApplicationDb, ISignInHelper signInHelper) : BaseRepository<Product, ProductVm, long>(mapper, context, dapperApplicationDb, signInHelper), IProductRepository
{

}
