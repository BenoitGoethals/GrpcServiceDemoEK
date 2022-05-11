using Grpc.Core;
using Lab03.ServerSide.Core.Interfaces;

namespace Lab03.ServerSide.Services {
    public class CalculatorGrpcService : Calculator.CalculatorBase {
        private readonly ILogger<CalculatorGrpcService> _logger;
        private readonly ICalculatorService calculatorService;

        public CalculatorGrpcService(ILogger<CalculatorGrpcService> logger, ICalculatorService calculatorService) {
            _logger = logger;
            this.calculatorService = calculatorService;
        }

        public override async Task<SumReply> Sum(IAsyncStreamReader<SumRequest> requestStream, ServerCallContext context) {
            
            List<int> numbers = new List<int>(); 
            await foreach (var item in requestStream.ReadAllAsync()) {
                numbers.Add(item.Number);
            }
            long sum = calculatorService.Sum(numbers);
            _logger.LogInformation("The sum of " + string.Join(",", numbers) + " is " + sum);
            return new SumReply() { Number = sum };
        }

        public override async Task PrimeFactors(
            IAsyncStreamReader<PrimeFactorsRequest> requestStream, 
            IServerStreamWriter<PrimeFactorsReply> responseStream, 
            ServerCallContext context) {
            List<int> numbers = new List<int>();
            await foreach (var item in requestStream.ReadAllAsync()) {
                numbers.Add(item.Number);
            }
            long sum = calculatorService.Sum(numbers);
            IEnumerable<long> primes = calculatorService.PrimeFactors(sum);
            _logger.LogInformation("The prime factorization of " + string.Join(",", numbers) + " (of which the sum is " + sum + ") is " + string.Join(",", primes));
            foreach (var prime in primes) {
                await responseStream.WriteAsync(new PrimeFactorsReply() { Number = prime });
            }
        }
    }
}