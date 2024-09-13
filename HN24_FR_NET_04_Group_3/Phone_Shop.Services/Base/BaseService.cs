using AutoMapper;
using Phone_Shop.DataAccess.UnitOfWorks;

namespace Phone_Shop.Services.Base
{
    public class BaseService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BaseService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
