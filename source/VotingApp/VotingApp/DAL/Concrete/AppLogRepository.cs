using VotingApp.DAL.Abstract;
using VotingApp.Models;

namespace VotingApp.DAL.Concrete
{
    public class AppLogRepository : IAppLogRepository
    {
        private VotingAppDbContext _context;

        public AppLogRepository(VotingAppDbContext ctx)
        {
            _context = ctx;
        }

        public AppLog AddOrUpdate(AppLog log)
        {
            if (log == null)
            {
                throw new NullReferenceException();
            }
            _context.AppLogs.Update(log);
            _context.SaveChanges();
            return log;
        }
        public AppLog LogInfo(string className, string methodName, string message)
        {
            var log = new AppLog
            {
                Date = DateTime.Now,
                LogLevel = "Info",
                ClassName = className,
                MethodName = methodName,
                LogMessage = message
            };

            AddOrUpdate(log);
            return log;
        }

        public AppLog LogError(string className, string methodName, string message)
        {
            var log = new AppLog
            {
                Date = DateTime.Now,
                LogLevel = "Error",
                ClassName = className,
                MethodName = methodName,
                LogMessage = message
            };

            AddOrUpdate(log);
            return log;
        }

        public IList<AppLog> GetAllAppLogs()
        {
            return _context.AppLogs.OrderByDescending(a => a.Date).ToList();
        }
    }
}
