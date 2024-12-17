﻿using SystemProductOrder.DTO;
using SystemProductOrder.models;

namespace SystemProductOrder.Servieses
{
    public interface IOrderServices
    {
        List<Order> GetAllOrders(int id);
        Order GetOrderById(int id);
        Task<bool> HasUserPurchasedProduct(int userId, int productId);
        void PlaceOrder(int userId, List<OrderProductInput> orderDetails);
    }
}