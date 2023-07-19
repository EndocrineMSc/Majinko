
namespace Utility
{
    public interface IDisplayOnScroll
    {
        public void DisplayOnScroll();
        public void StopDisplayOnScroll();

        public string DisplayDescription { get; set; }

        public int DisplayScale { get; set; }
    }
}
