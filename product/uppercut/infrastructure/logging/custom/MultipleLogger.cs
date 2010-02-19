namespace uppercut.infrastructure.logging.custom
{
    using System.Collections.Generic;

    public class MultipleLogger : ILog
    {
        private readonly IList<ILog> the_loggers;

        public MultipleLogger(IList<ILog> loggers)
        {
            the_loggers = loggers ?? new List<ILog>();
        }


        public void log_a_debug_event_containing(string message, params object[] args)
        {
            foreach (ILog logger in the_loggers)
            {
                logger.log_a_debug_event_containing(message, args);
            }
        }

        public void log_an_info_event_containing(string message, params object[] args)
        {
            foreach (ILog logger in the_loggers)
            {
                logger.log_an_info_event_containing(message, args);
            }
        }

        public void log_a_warning_event_containing(string message, params object[] args)
        {
            foreach (ILog logger in the_loggers)
            {
                logger.log_a_warning_event_containing(message, args);
            }
        }

        public void log_an_error_event_containing(string message, params object[] args)
        {
            foreach (ILog logger in the_loggers)
            {
                logger.log_an_error_event_containing(message, args);
            }
        }

        public void log_a_fatal_event_containing(string message, params object[] args)
        {
            foreach (ILog logger in the_loggers)
            {
                logger.log_a_fatal_event_containing(message, args);
            }
        }
    }
}