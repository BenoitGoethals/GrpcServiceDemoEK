using Lab02.Server.Core.Interfaces;
using Lab02.Server.Services;

namespace Lab02.Server.Core;
public class KeysGenerator : IKeysGenerator {
    public IEnumerable<string> GenerateKeys(RegisterRequest request) {
        Random random = new Random();
        yield return $"{request.Name}{random.Next(0, 100):000}";
        foreach (string member in request.FamilyMembers) {
            yield return $"{member}{random.Next(0, 100):000}";
        }
    }
}
