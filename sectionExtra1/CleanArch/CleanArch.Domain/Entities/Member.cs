
using System.Text.Json.Serialization;
using CleanArch.Domain.Validation;

namespace CleanArch.Domain.Entities;

public sealed class Member : Entity
{
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public string? Gender { get; private set; }
    public string? Email { get; private set; }
    public bool? IsActive { get; private set; }

    public Member(string firstName, string lastName, string gender, string email, bool? active)
    {
        ValidateDomain(firstName, lastName, gender, email, active);
    }

    [JsonConstructor]
    public Member(int id, string firstName, string lastName, string gender, string email, bool? active)
    {
        DomainValidation.When(id < 0, "Invalid Id");
        Id = id;
        ValidateDomain(firstName, lastName, gender, email, active);
    }

    public Member() { }

    public void Update(string firstName, string lastName, string gender, string email, bool? active)
    {
        ValidateDomain(firstName, lastName, gender, email, active);
    }

    private void ValidateDomain(string firstName, string lastName, string gender, string email, bool? active)
    {
        DomainValidation.When(string.IsNullOrEmpty(firstName), "First name is required");
        DomainValidation.When(firstName.Length < 3, "First name must be at least 3 characters");

        DomainValidation.When(string.IsNullOrEmpty(lastName), "Last name is required");
        DomainValidation.When(lastName.Length < 3, "Last name must be at least 3 characters");

        DomainValidation.When(string.IsNullOrEmpty(gender), "Gender is required");

        DomainValidation.When(string.IsNullOrEmpty(email), "Email is required");
        DomainValidation.When(email.Length > 250, "Email must be at most 250 characters");
        DomainValidation.When(email.Length < 6, "Email must be at least 6 characters");

        DomainValidation.When(!active.HasValue, "Member must be active");

        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        Email = email;
        IsActive = active;
    }
}
