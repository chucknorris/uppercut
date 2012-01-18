namespace uppercut.tests.infrastructure.logging
{
    using bdddoc.core;
    using developwithpassion.bdd;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;
    using Rhino.Mocks;
    using uppercut.infrastructure.containers;
    using uppercut.infrastructure.containers.custom;
    using uppercut.infrastructure.logging;

    public class LogSpecs
    {
        public abstract class concern_for_logging : observations_for_a_static_sut
        {
            protected static ILog result;

            protected static IContainer the_container;
            protected static ILogFactory mock_log_factory;
            protected static ILog mock_logger;

            private context c = () =>
                                    {
                                        the_container = an<IContainer>();
                                        Container.initialize_with(the_container);
                                    };

            private after_each_observation after = () => { Container.initialize_with(null); };
        }

        [Concern(typeof (Log))]
        public class when_asking_for_a_logger_and_one_has_been_registered : concern_for_logging
        {
            private context c = () =>
                                    {
                                        mock_log_factory = an<ILogFactory>();
                                        mock_logger = an<ILog>();
                                        the_container.Stub(x => x.Resolve<ILogFactory>())
                                            .Return(mock_log_factory);
                                        mock_log_factory.Stub(x => x.create_logger_bound_to(typeof (WindsorContainer)))
                                            .IgnoreArguments()
                                            .Return(mock_logger);
                                        //when(the_container).is_told_to(x => x.Resolve<ILogFactory>())
                                        //    .Return(mock_log_factory);
                                        //when_the(mock_log_factory).is_told_to(x => x.create_logger_bound_to(typeof(WindsorContainer)))
                                        //    .IgnoreArguments()
                                        //    .Return(mock_logger);
                                    };

            private because b = () => { result = Log.bound_to(typeof (WindsorContainer)); };

            [Observation]
            public void should_have_called_the_container_to_resolve_a_registered_logger()
            {
                the_container.was_told_to(x_ => x_.Resolve<ILogFactory>());
            }

            [Observation]
            public void should_not_be_null()
            {
                result.should_not_be_null();
            }

            [Observation]
            public void should_return_a_custom_logger()
            {
                //mock_log_factory.should_be_equal_to(result);
                //result.should_be_an_instance_of<ILogFactory>();
                // result.should_be_equal_to(mock_log_factory);
            }
        }
    }
}