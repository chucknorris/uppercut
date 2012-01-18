namespace uppercut.infrastructure.logging.custom
{
    using System;
    using containers;

    public class MultipleLoggerLogFactory : ILogFactory
    {
        public ILog create_logger_bound_to(Object type)
        {
            return Container.get_an_instance_of<ILog>();
        }
        
    }
}