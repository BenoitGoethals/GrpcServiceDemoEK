using Lab02.Server.Core.Interfaces;

namespace Lab02.Server.Core;
public class AgeCalculator : IAgeCalculator {
    private IDateProvider dateProvider;

    public AgeCalculator(IDateProvider dateProvider) {
        this.dateProvider = dateProvider;
    }

    public int CalculateAge(DateTime birthDate) {
        DateTime now = dateProvider.Now;
        int age = now.Year - birthDate.Year;

        // For leap years we need this
        if (birthDate > now.AddYears(-age))
            age--;
        // Don't use:
        // if (birthDate.AddYears(age) > now) 
        //     age--;

        return age;
    }
}
