namespace uppercut.tasks
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using NAnt.Core;
    using NAnt.Core.Attributes;

    /// <summary>
    /// The <see cref="LogLevelTask"/>
    /// class is a NAnt task that is used to determine the logging level used to execute
    /// a set of child tasks.http://www.neovolve.com/post/2008/01/16/loglevel-nant-task.aspx
    /// </summary>
    /// <remarks>This code was inspired from the blog post found at http://jayflowers.com/WordPress/?p=133</remarks>
    [TaskName("loglevel")]
    public class LogLevelTask : TaskContainer
    {

        /// <summary>
        /// Stores the LogLevel value.
        /// </summary>
        private Level _logLevel;

        /// <summary>
        /// Executes the task.
        /// </summary>
        protected override void ExecuteTask()
        {
            Level OldLevel = Project.Threshold;

            AssignLogLevel(LogLevel);

            base.ExecuteTask();

            AssignLogLevel(OldLevel);
        }

        /// <summary>
        /// Assigns the log level.
        /// </summary>
        /// <param name="newLevel">The new level.</param>
        private void AssignLogLevel(Level newLevel)
        {
            // Loop through each logger
            foreach (IBuildListener listener in Project.BuildListeners)
            {
                IBuildLogger logger = listener as IBuildLogger;

                // Assign the new threshold
                if (logger != null)
                {
                    logger.Threshold = newLevel;
                }
            }
        }

        /// <summary>
        /// Gets or sets the log level.
        /// </summary>
        /// <value>The log level.</value>
        [TaskAttribute("level", Required = true)]
        public Level LogLevel
        {
            get { return _logLevel; }
            set { _logLevel = value; }
        }
    }
}