using Lab02.Server.Core.Interfaces;
using Lab02.Server.Services;

namespace Lab02.Server.Core;
public class PriceCalculator : IPriceCalculator {
    public uint PriceMultiplier(MemberType memberType) => memberType switch {
        MemberType.Basic => 1,
        MemberType.Gold => 2,
        MemberType.Platinum => 3,
        _ => throw new ArgumentException("Please Specify a Subscription Type")
    };
    public uint BasicPrice(MemberType memberType) => memberType switch {
        MemberType.Basic => 10,
        MemberType.Gold => 15,
        MemberType.Platinum => 20,
        _ => throw new ArgumentException("Please Specify a Subscription Type")
    };
    public uint PriceWithoutShipping(MemberType memberType, uint familyMembers) => BasicPrice(memberType) + (familyMembers * PriceMultiplier(memberType));

    public uint FinalPrice(MemberType memberType, uint familyMembers, RegisterRequest.ContactOnOneofCase contactOn) {
        if (contactOn == RegisterRequest.ContactOnOneofCase.None)
            throw new ArgumentException("Please specify how to be contacted");
        uint price = PriceWithoutShipping(memberType, familyMembers);
        if (contactOn == RegisterRequest.ContactOnOneofCase.Address)
            price += 5;
        return price;
    }

    public uint FinalPrice(RegisterRequest request) => FinalPrice(request.SubscriptionType, (uint)request.FamilyMembers.Count, request.ContactOnCase);
    
}
