namespace uppercut.infrastructure.containers.custom
{
    using Castle.Windsor;

    public class WindsorContainer : IContainer
    {
        private readonly IWindsorContainer the_container;

        public WindsorContainer(IWindsorContainer the_container)
        {
            this.the_container = the_container;
        }

        public TypeToReturn Resolve<TypeToReturn>()
        {
            return the_container.Resolve<TypeToReturn>();
        }
    }
}