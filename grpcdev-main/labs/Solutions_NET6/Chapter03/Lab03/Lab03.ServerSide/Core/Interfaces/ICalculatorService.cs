namespace Lab03.ServerSide.Core.Interfaces; 
public interface ICalculatorService {
    IEnumerable<long> PrimeFactors(long number);
    long Sum(IEnumerable<int> numbers);
}
