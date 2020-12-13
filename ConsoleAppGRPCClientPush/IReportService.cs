using System.Threading.Tasks;

namespace ConsoleAppGRPCClientPush
{
    public interface IReportService
    {
        Task AddReport(Salsa Salsa);
        public void InitConnection();
        public  Task CloseConnectio();
    }
}