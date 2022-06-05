using VotingApp.Models;

namespace VotingApp.DAL.Abstract
{
    public interface IAppLogRepository
    {
        public AppLog AddOrUpdate(AppLog log);
        public AppLog LogInfo(string className, string methodName, string message);
        public AppLog LogError(string className, string methodName, string message);
        public IList<AppLog> GetAllAppLogs();
    }
}
