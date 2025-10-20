namespace CartApp.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public ICollection<CartItem> Items { get; set; }
    }
}
