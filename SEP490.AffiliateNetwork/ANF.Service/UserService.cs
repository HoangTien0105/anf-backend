using ANF.Core;
using ANF.Core.Services;

namespace ANF.Service
{
    public class UserService(IUnitOfWork unitOfWork) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

    }
}
