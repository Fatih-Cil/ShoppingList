﻿using ShoppingList.Application.Abstractions.IRepositories;
using ShoppingList.Domain.Entities;
using ShoppingList.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingList.Persistence.Concrate.Repositories
{
    public class EfProductRepository : EfEntityRepositoryBase<Product, ShoppingListContext>, IProductRepository
    {
    }
}
