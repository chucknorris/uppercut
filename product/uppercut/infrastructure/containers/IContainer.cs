namespace uppercut.infrastructure.containers
{
    public interface IContainer
    {
        TypeToReturn Resolve<TypeToReturn>();
    }
}