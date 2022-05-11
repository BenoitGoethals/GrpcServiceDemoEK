using Lab02.Server.Core;
using Lab02.Server.Services;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
namespace Lab02.Server.Tests.Core;
public class KeysGeneratorTests {
    [Fact]
    public void GenerateKeys_GeneratesAsManyKeysAsTheFamilyMembers() {
        RegisterRequest request = new() { Name = "Main"};
        request.FamilyMembers.Add("First");
        request.FamilyMembers.Add("Second");
        request.FamilyMembers.Add("Third");

        KeysGenerator keysGenerator = new KeysGenerator();
        List<string> actual = keysGenerator.GenerateKeys(request).ToList();

        Assert.Collection(actual,
            item => Assert.Matches(@"Main\d\d\d", item),
            item => Assert.Matches(@"First\d\d\d", item),
            item => Assert.Matches(@"Second\d\d\d", item),
            item => Assert.Matches(@"Third\d\d\d", item)
        );
    }
}
