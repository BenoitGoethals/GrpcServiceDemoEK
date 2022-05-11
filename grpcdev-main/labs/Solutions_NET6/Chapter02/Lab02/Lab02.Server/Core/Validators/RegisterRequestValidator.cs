using FluentValidation;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Lab02.Server.Core.Interfaces;
using Lab02.Server.Services;

namespace Lab02.Server.Core.Validators; 
public class RegisterRequestValidator: AbstractValidator<RegisterRequest> {
    private readonly IAgeCalculator ageCalculator;

    public RegisterRequestValidator(IAgeCalculator ageCalculator) {
        this.ageCalculator = ageCalculator;
        RuleFor(x => x.FamilyMembers).Must(BeLessOrEqualThan5);
        RuleFor(x => x.BirthDate).Must(BeOver18YearsOld);
    }

    private bool BeOver18YearsOld(Timestamp arg) => ageCalculator.CalculateAge(arg.ToDateTime()) >= 18;
    
    private bool BeLessOrEqualThan5(RepeatedField<string> arg) => arg.Count <= 5;
}
