namespace DAL.Constants;

public class AuthorizationConst
{
    public enum Roles
    {
        Administrator,
        Moderator,
        User
    }
    public const string default_username = "defaultUser";
    public const string default_email = "defusr@example.com";
    public const string default_password = "defusr";
    public const Roles default_role = Roles.User;
}