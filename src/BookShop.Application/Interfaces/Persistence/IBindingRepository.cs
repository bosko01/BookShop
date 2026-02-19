using BookShop.Application.Interfaces.Persistence.Common;
using System;
using System.Collections.Generic;
using System.Text;
using BindingEntity = BookShop.Domain.Entities.Binding;

namespace BookShop.Application.Interfaces.Persistence
{
    public interface IBindingRepository : IRepository<BindingEntity, int>
    {

    }
}
