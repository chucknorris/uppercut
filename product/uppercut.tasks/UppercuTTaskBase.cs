namespace uppercut.tasks
{
    using System.Collections.Generic;
    using Castle.Windsor;
    using infrastructure.containers;
    using infrastructure.filesystem;
    using infrastructure.logging;
    using infrastructure.logging.custom;
    using log4net;
    using NAnt.Core;
    using ILog = log4net.ILog;

    public abstract class UppercuTTaskBase : Task
    {
        private readonly ILog the_logger = LogManager.GetLogger(typeof(UppercuTTaskBase));

        protected IContainer build_the_container()
        {
            IWindsorContainer windsor_container = new WindsorContainer();

            infrastructure.logging.ILog nant_logger = new NAntLogger(this);
            infrastructure.logging.ILog log4net_logger = new Log4NetLogger(the_logger);
            infrastructure.logging.ILog multi_logger = new MultipleLogger(new List<infrastructure.logging.ILog> { nant_logger, log4net_logger });

            windsor_container.Kernel.AddComponentInstance<infrastructure.logging.ILog>(multi_logger);
            windsor_container.AddComponent<IFileSystemAccess, WindowsFileSystemAccess>();
            windsor_container.AddComponent<ILogFactory, MultipleLoggerLogFactory>();

            return new infrastructure.containers.custom.WindsorContainer(windsor_container);
        }

        protected override void ExecuteTask()
        {
            Container.initialize_with(build_the_container());
            set_up_properties();
            run_the_task();
        }

        public abstract void set_up_properties();
        public abstract void run_the_task();
    }
}