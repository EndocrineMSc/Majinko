namespace Characters
{
    public interface ICanBeIntangible
    {
        int IntangibleStacks { get; set; }
        void SetIntangible(int intangibleStacks = 1);
        void RemoveIntangible();
        void HandleIntangibleStacks();
    }
}
