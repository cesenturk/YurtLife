namespace YurtLife.Models
{
    public class DefaultFilter
    {
        public string SearchString { get; set; }
        public ProductColumnEnum Sort { get; set; }
        public SortDir SortDir { get; set; }
    }
}