﻿using Bakery.Models.Items;

namespace Bakery.Service
{
    public interface IItemsService
    {
        DetailsViewModel GetDetails(int id);
    }
}
