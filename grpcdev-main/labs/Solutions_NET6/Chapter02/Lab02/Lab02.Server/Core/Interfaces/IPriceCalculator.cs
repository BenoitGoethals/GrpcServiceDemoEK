using Lab02.Server.Services;

namespace Lab02.Server.Core.Interfaces; 
public interface IPriceCalculator {
    uint BasicPrice(MemberType memberType);
    uint PriceMultiplier(MemberType memberType);
    uint PriceWithoutShipping(MemberType memberType, uint familyMembers);
    uint FinalPrice(MemberType memberType, uint familyMembers, RegisterRequest.ContactOnOneofCase contactOn);
    uint FinalPrice(RegisterRequest request);
}
