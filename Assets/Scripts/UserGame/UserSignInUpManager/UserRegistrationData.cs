public class UserRegistrationData
{
    public string Email { get; set; }
    public string UserName { get; set; }
    public string Pass { get; set; }
    public bool autoLogin { get; set; }
    public UserRegistrationData(string Email, string UserName, string Pass)
    {
        this.Email = Email;
        this.UserName = UserName;
        this.Pass = Pass;
    }

    public UserRegistrationData(string UserName, string Pass)
    {
        this.UserName = UserName;
        this.Pass = Pass;
    }
}
