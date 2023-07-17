using Mock_AWSRDS_API.Interface;
using Mock_AWSRDS_API.Models;
using Mock_AWS_API.Repos;
using System.Transactions;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Runtime.InteropServices;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mock_AWS_API.Services
{
    public class MockService : IMockService
    {
        private readonly ILogger<MockService> _logger;
        private readonly MyDbContext _dbContext;
        private readonly IPasswordHelper _passwordHelper;

        public MockService(ILogger<MockService> logger, MyDbContext dbContext, IPasswordHelper passwordHelper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _passwordHelper = passwordHelper;
        }

        public bool AddUser (string userName, string password)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    User? exist = this._dbContext.Users.Where(u => u.Username == userName).FirstOrDefault();
                    if (exist == null)
                    {
                        // Generate a random salt
                        byte[] salt = this._passwordHelper.GenerateSalt();

                        // Hash the password with the generated salt
                        byte[] hashedPassword = this._passwordHelper.HashPassword(password, salt);

                        // Convert the byte arrays to Base64 strings for storage
                        string saltString = Convert.ToBase64String(salt);
                        string hashedPasswordStr = Convert.ToBase64String(hashedPassword);

                        User user = new User()
                        {
                            Username = userName,
                            HashPassword = hashedPasswordStr,
                            SaltString = saltString
                        };

                        this._dbContext.Users.Add(user);

                        if (this._dbContext.ChangeTracker.HasChanges())
                        {
                            this._dbContext.SaveChanges();
                            transaction.Complete();
                            return true;
                        }

                        return false;
                    }

                    _logger.LogInformation("User already exists");
                    return false;
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occured: {ex.Message} ");
                    transaction.Dispose();
                    return false;
                }
            }
        }

        public string Login(string userName, string password)
        {
            try
            {
                User? exist = this._dbContext.Users.Where(u => u.Username == userName).FirstOrDefault();
                if (exist != null)
                {
                    if (this._passwordHelper.ValidatePassword(password, exist.SaltString, exist.HashPassword))
                    {
                        int random = new Random().Next(1000);
                        return this._passwordHelper.GenerateJwtToken(this._passwordHelper.RandomString(random), userName, "self", userName);
                    }

                    _logger.LogInformation("Invalid password");
                    return string.Empty;
                }

                _logger.LogInformation("User does not exist");
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured: {ex.Message} ");
                
                return string.Empty;
            }
        }
    

        public bool InserJonin(Jonin jonin)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    _logger.LogInformation("Adding jonin to databse...");
                    this._dbContext.Jonins.Add(jonin);

                    if (this._dbContext.ChangeTracker.HasChanges())
                    {
                        _logger.LogInformation("Saving changes...");

                        this._dbContext.SaveChanges();
                        transaction.Complete();

                        _logger.LogInformation("Jonin saved and transaction committed.");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occured: {ex.Message} ");
                    transaction.Dispose();
                    return false;

                }
            }
            _logger.LogWarning("Could not establish transaction scope");
            return false;
        }

        public bool DeleteJoninByName(Guid id)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    _logger.LogInformation($"Deleting joning...");
                    Jonin? delete = this._dbContext.Jonins.Where(j => j.Id == id).FirstOrDefault();
                    if (delete != null)
                    {
                        _logger.LogInformation($"Retreived jonin with id {id}...");
                        this._dbContext.Jonins.Remove(delete);

                        if (this._dbContext.ChangeTracker.HasChanges())
                        {
                            this._dbContext.SaveChanges();
                            transaction.Complete();
                            return true;
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"Jonin does not exists");
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occured: {ex.Message} ");
                    transaction.Dispose();
                    return false;
                }
            }
            _logger.LogWarning("Could not establish transaction scope");
            return false;
        }

        public bool UpdateJoninByName(Guid id, string? firstName = "", string? lastName = "", DateTime? birthDay = null)
        {
            using (var transaction = new TransactionScope())
            {
                try
                {
                    _logger.LogInformation($"Retrieving Jonin with id {id}...");
                    Jonin? update = this._dbContext.Jonins.Where(j => j.Id == id).FirstOrDefault();
                    if (update != null)
                    {
                        _logger.LogInformation($"Updating Jonin...");

                        if (!string.IsNullOrEmpty(firstName))
                            update.FirstName = firstName;

                        if (!string.IsNullOrEmpty(lastName))
                            update.LastName = lastName;

                        if (birthDay.HasValue)
                            update.BirthDate = birthDay.Value;

                        this._dbContext.Jonins.Update(update);

                        if (this._dbContext.ChangeTracker.HasChanges())
                        {
                            this._dbContext.SaveChanges();
                            transaction.Complete();
                            return true;
                        }
                    }
                    else
                    {
                        _logger.LogInformation($"Jonin does not exists");
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occured: {ex.Message} ");
                    transaction.Dispose();
                    return false;
                }
            }
            _logger.LogWarning("Could not establish transaction scope");
            return false;
        }

        public List<Genin> GetAllGenin()
        {
            throw new NotImplementedException();
        }

        public List<Jonin> GetAllJonin()
        {
            throw new NotImplementedException();
        }

     
    }


}
