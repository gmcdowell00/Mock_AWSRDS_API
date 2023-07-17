using Mock_AWSRDS_API.Models;

namespace Mock_AWSRDS_API.Interface
{
    public interface IMockService
    {
        bool AddUser(string userName, string password);
        string Login(string userName, string password);

        List<Genin> GetAllGenin();
        List<Jonin> GetAllJonin();

        bool InserJonin(Jonin jonin);

        bool DeleteJoninByName(Guid id);

        bool UpdateJoninByName(Guid id, string? firstName, string? lastName, DateTime? birthDay);

        


    }
}

