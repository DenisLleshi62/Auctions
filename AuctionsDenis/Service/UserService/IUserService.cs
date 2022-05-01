using AuctionsDenis.Controllers;
using AuctionsProject.Data;
using AuctionsProject.Models;
using WebApi.Models.Users;

namespace AuctionsDenis.Service.UserService
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Users> GetAll();
        Users GetById(int id);
        void Register(RegisterRequest model);
        void Update(int id, UpdateRequest model);
        void Delete(int id);
        Wallet GetWallet(int userId);

    }
}