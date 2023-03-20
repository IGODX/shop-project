using MyShopPet.Data;

namespace MyShopPet.Models.Domain
{
    public class Cart
    {
        List<CartItem> cartItems;

        public IEnumerable<CartItem> CartItems => cartItems;

        public Cart()
        {
            cartItems = new List<CartItem>();
        }
        public Cart(IEnumerable<CartItem> cartItems)
        {
            this.cartItems = cartItems.ToList();
        }
        public void AddToCard(Product product, int count)
        {
            CartItem? item = cartItems.FirstOrDefault(e => e.Product.Id == product.Id);
            if (item != null)
                item.Count += count;
            else
                cartItems.Add(new CartItem { Product = product, Count = count });
        }
        public void RemoveFromCart(Product product)
        {
            cartItems.RemoveAll(e => e.Product.Id == product.Id);
        }
        public void Clear()
        {
            cartItems.Clear();
        }
        public double TotalPrice => cartItems.Sum(e => e.Product.Price * e.Count);
    }
     
}
