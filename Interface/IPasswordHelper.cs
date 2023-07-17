namespace Mock_AWSRDS_API.Interface
{
    public interface IPasswordHelper
    {
       
        byte[] GenerateSalt();
        bool ValidatePassword(string password, string saltString, string hashedPasswordString);
        byte[] HashPassword(string password, byte[] salt);
        string RandomString(int length);

        string GenerateJwtToken(string secretKey, string issuer, string audience, string username);
    }
}
