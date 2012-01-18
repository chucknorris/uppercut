namespace uppercut.tests.infrastructure.containers
{
    using System;
    using bdddoc.core;
    using Castle.Windsor;
    using developwithpassion.bdd.contexts;
    using developwithpassion.bdd.mbunit;
    using developwithpassion.bdd.mbunit.standard;
    using developwithpassion.bdd.mbunit.standard.observations;
    using Rhino.Mocks;
    using uppercut.infrastructure.containers;
    using uppercut.infrastructure.logging;
    using uppercut.infrastructure.logging.custom;

    public class ContainerSpecs
    {
        public abstract class concern_for_container : observations_for_a_static_sut
        {
            protected static ILogFactory result;
            protected static IContainer the_container;

            context c = () =>
                            {
                                the_container = an<IContainer>();
                                Container.initialize_with(the_container);
                            };

            after_each_observation a = () => Container.initialize_with(null);
        }

        [Concern(typeof(Container))]
        public class when_asking_the_container_to_initialize : concern_for_container
        {
            because b = () => { result = Container.get_an_instance_of<ILogFactory>(); };

            [Observation]
            public void should_not_be_of_type_IWindsorContainer()
            {
                the_container.should_not_be_an_instance_of<IWindsorContainer>();
            }
        }

        [Concern(typeof(Container))]
        public class when_asking_the_container_to_resolve_an_item_and_it_has_the_item_registered : concern_for_container
        {
            context c = () => the_container.Stub(x => x.Resolve<ILogFactory>()).Return(new Log4NetLogFactory());

            because b = () => { result = Container.get_an_instance_of<ILogFactory>(); };

            [Observation]
            public void should_be_an_instance_of_Log4NetLogFactory()
            {
                result.should_be_an_instance_of<Log4NetLogFactory>();
            }

            [Observation]
            public void should_be_an_instance_of_ILogFactory()
            {
                result.should_be_an_instance_of<ILogFactory>();
            }
        }

        [Concern(typeof(Container))]
        public class when_asking_the_container_to_resolve_an_item_and_it_does_not_have_the_item_registered : concern_for_container
        {
            static Action attempting_to_get_an_unregistered_item;

            context c = () => the_container.Stub(x => x.Resolve<ILogFactory>()).Throw(
                                  new Exception(string.Format("Had an error finding components registered for {0}.", typeof(ILogFactory))));

            because b = () => { attempting_to_get_an_unregistered_item = () => the_container.Resolve<ILogFactory>(); };

            [Observation]
            public void should_throw_an_exception()
            {
                attempting_to_get_an_unregistered_item.should_throw_an<Exception>();
            }
        }
    }
}