using Lab02.Server.Services;

namespace Lab02.Server.Core.Interfaces; 
public interface IPictureRepository {
    void SavePicture(RegisterRequest request);
}
