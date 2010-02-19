namespace uppercut.infrastructure.logging.custom
{
    using System;
    using log4net;
    using ILog=uppercut.infrastructure.logging.ILog;

    public class Log4NetLogFactory : ILogFactory
    {
        public ILog create_logger_bound_to(Object type)
        {
            return new Log4NetLogger(LogManager.GetLogger(type.ToString()));
        }
    }
}