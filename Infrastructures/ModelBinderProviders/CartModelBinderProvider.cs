using MyShopPet.Infrastructures.ModelBinders;
using MyShopPet.Models.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MyShopPet.Infrastructures.ModelBinderProviders
{
    public class CartModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            return context.Metadata.ModelType == typeof(Cart) ? new CartModelBinder() : null;
        }
    }
}
