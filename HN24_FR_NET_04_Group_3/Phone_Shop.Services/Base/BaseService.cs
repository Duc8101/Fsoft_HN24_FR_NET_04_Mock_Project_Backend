using AutoMapper;
using Phone_Shop.DataAccess.UnitOfWorks;

namespace Phone_Shop.Services.Base
{
    public class BaseService
    {

        private protected readonly IUnitOfWork _unitOfWork;
        private protected readonly IMapper _mapper;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
