namespace TravelPlanner.Web
{
    public class PasswordService
    {
        public string GetHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool IsValid(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
