using FluentValidation;
using FluentValidation.Validators;
using knowledgeBase.Entities;

namespace knowledgeBase;

public class UserRegisterValidator : AbstractValidator<User>
{
    public UserRegisterValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
            .NotNull().WithMessage("Почта введена не корректно!");
        RuleFor(u => u.Password)
            .MatchPassword()
            .WithMessage("Пароль должен иметь длину от 2 до 15 символов");
        RuleFor(u => u.Name)
            .NotEmpty()
            .MatchName().WithMessage("Имя не соответствует требованиям. Оно должно содержать только латинские буквы и цифры");
    }
}

public class UserLoginValidator : AbstractValidator<User>
{
    public UserLoginValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
            .NotNull().WithMessage("Почта введена не корректно!");
        RuleFor(u => u.Password)
            .MatchPassword()
            .WithMessage("Пароль должен иметь длину от 2 до 15 символов");
    }
}



public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string> MatchPassword<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new RegularExpressionValidator<T>(@"^(?=.*[a-zA-Z])(?=.*\d)[a-zA-Z\d]{5,20}$"));
    }
    
    public static IRuleBuilderOptions<T, string> MatchName<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new RegularExpressionValidator<T>(@"^[a-zA-Z0-9]{2,15}$"));
    }
    
    public static IRuleBuilderOptions<T, string> MatchPostalCode<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder.SetValidator(new RegularExpressionValidator<T>(@"\d{6}$"));
    }
}