namespace RefactoringChallenge.Domain;

public class Customer : Entity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public bool IsVip { get; set; }
    public DateTime RegistrationDate { get; set; }
}
