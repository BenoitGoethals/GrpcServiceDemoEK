using Lab02.Server.Services;

namespace Lab02.Server.Core.Interfaces; 
public interface IKeysGenerator {
    IEnumerable<string> GenerateKeys(RegisterRequest request);
}
