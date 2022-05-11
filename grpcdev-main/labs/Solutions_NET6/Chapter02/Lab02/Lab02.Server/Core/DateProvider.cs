using Lab02.Server.Core.Interfaces;

namespace Lab02.Server.Core {
    public class DateProvider : IDateProvider {
        public DateTime Now => DateTime.Now;
    }
}
