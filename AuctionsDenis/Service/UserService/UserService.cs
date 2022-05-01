
using AuctionsDenis.Service.UserService;
using BCryptNet = BCrypt.Net.BCrypt;
using AuctionsProject.Data;
using AuctionsProject.Models;
using MapsterMapper;
using WebApi.Authorization;
using WebApi.Helpers;
using WebApi.Models.Users;

namespace AuctionsDenis.Service.UserService;

public class UserService : IUserService
{

    private AuctionsContext _context;
        private IJwtUtils _jwtUtils;
        private readonly IMapper _mapper;

        public UserService(
            AuctionsContext context,
            IJwtUtils jwtUtils,
            IMapper mapper)
        {
            _context = context;
            _jwtUtils = jwtUtils;
            _mapper = mapper;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _context.Users.FirstOrDefault(x => x.UserName == model.UserName);

            // validate
            if (user == null || !BCryptNet.Verify(model.Password, user.PasswordHash))
                throw new AppException("Username or password is incorrect");

            // authentication successful
            var response = _mapper.Map<AuthenticateResponse>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public IEnumerable<Users> GetAll()
        {
            return _context.Users;
        }

        public Users GetById(int id)
        {
            return getUser(id);
        }

        public void Register(RegisterRequest model)
        {
            // validate
            if (_context.Users.Any(x => x.UserName == model.UserName))
                throw new AppException("Username '" + model.UserName + "' is already taken");

            // map model to new user object
            var user = _mapper.Map<Users>(model);

            // hash password
            user.PasswordHash = BCryptNet.HashPassword(model.Password);
           
            // save user
            _context.Users.Add(user);
           
            
            _context.SaveChanges();
            
            var wallet = new Wallet();
            wallet.Amount=(decimal)1000.00;
            wallet.UsableAmount=(decimal)1000.00;
            wallet.UserId = user.UserId;
            _context.Wallet.Add(wallet);
            _context.SaveChanges();
            
        }

        public void Update(int id, UpdateRequest model)
        {
            var user = getUser(id);

            // validate
            if (model.UserName != user.UserName && _context.Users.Any(x => x.UserName == model.UserName))
                throw new AppException("Username '" + model.UserName + "' is already taken");

            // hash password if it was entered
            if (!string.IsNullOrEmpty(model.Password))
                user.PasswordHash = BCryptNet.HashPassword(model.Password);
            if (string.IsNullOrEmpty(model.FirstName))
                model.FirstName = user.FirstName;
            if (string.IsNullOrEmpty(model.LastName))
                model.LastName=user.LastName;
            if (string.IsNullOrEmpty(model.UserName))
                model.UserName = user.UserName;

            // copy model to user and save
            _mapper.Map(model, user);
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = getUser(id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
        public Wallet GetWallet(int userId)
        {
            var wallet = (from w in _context.Set<Wallet>()
                where w.UserId == userId
                select w).First();

            return wallet;
        }
        // helper methods
        private Users getUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
}