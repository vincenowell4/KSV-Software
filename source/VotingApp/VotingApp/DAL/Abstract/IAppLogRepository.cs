using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface IAppLogRepository
    {
        public AppLog AddOrUpdate(AppLog log);
        public AppLog LogInfo(string message);
        public AppLog LogError(string message);
        public IList<AppLog> GetAllAppLogs();
    }
}
