using MyShopPet.Extensions;
using MyShopPet.Models.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyShopPet.Infrastructures.ModelBinders
{
    public class CartModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException();
            const string SessionKey = "cart";
            IEnumerable<CartItem>? cartItems = null;
            if(bindingContext.HttpContext.Session != null)
            {
                cartItems = bindingContext.HttpContext.Session.Get<IEnumerable<CartItem>>(SessionKey);
            }
            if (cartItems == null)
            {
                cartItems = new List<CartItem>();
                bindingContext.HttpContext.Session!.Set(SessionKey, cartItems);
            }
            Cart cart = new(cartItems);
            bindingContext.Result = ModelBindingResult.Success(cart);
            return Task.CompletedTask;
        }
    }
}
