﻿using BakeryData.Models;
using BakeryServices.Models.EditItem;
using BakeryServices.Models.Items;

namespace BakeryServices.Service.Items
{
    public interface IItemService
    {
        Task<DetailsViewModel> GetDetails(int id, string userId);

        Task<Item> FindItem(string name, int quantity, decimal currPrice);

        Task<List<EditItemsFormModel>> GetAllItems(int id);

        Task<Order> FindOrderById(int id);

        Task<Order> FindOrderByUserId(string userId);

        Task<Item> FindItemById(int id);

        Task ChangeItemQuantity(EditItemDataModel model);

        Task DeleteItem(Item item, Order order);

        Task DeleteAllItems(Order order);        
    }
}
