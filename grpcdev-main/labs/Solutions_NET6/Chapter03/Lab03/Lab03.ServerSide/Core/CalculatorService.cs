using Lab03.ServerSide.Core.Interfaces;

namespace Lab03.ServerSide.Core {
    public class CalculatorService : ICalculatorService {
        public long Sum(IEnumerable<int> numbers) => numbers.Sum();

        public IEnumerable<long> PrimeFactors(long number) {
            for (long factor = 2; number > 1; factor++) {
                if (number % factor == 0) {
                    while (number % factor == 0) {
                        number /= factor;
                        yield return factor;
                    }
                }
            }
        }
    }
}
