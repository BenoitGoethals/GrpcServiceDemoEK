using Lab02.Server.Core.Interfaces;
using Lab02.Server.Services;

namespace Lab02.Server.Infrastructure; 
public class PictureRepository : IPictureRepository {
    public void SavePicture(RegisterRequest request) {
        string filename = Path.Combine(Directory.GetCurrentDirectory(), request.Name.Replace(" ", "_") + ".png");
        using FileStream fs = File.Create(filename);
        request.Picture.WriteTo(fs);
    }
}
