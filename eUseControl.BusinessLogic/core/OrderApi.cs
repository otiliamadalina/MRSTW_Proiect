using System;
using System.Collections.Generic;
using System.Linq;
using eUseControl.BusinessLogic.DBModel;
using eUseControl.BusinessLogic.Interface;
using eUseControl.Domain.Entities;
using eUseControl.Domain.Entities.Order;

namespace eUseControl.BusinessLogic.Core
{
    public class OrderApi : UserServices, IOrderApi
    {
        private readonly List<ODbTable> orders = new List<ODbTable>();

        public bool CreateOrder(ODbTable order)
        {
            return CreateNewOrderAction(order);
        }

        public List<ODbTable> GetAllOrders()
        {
            using (var context = new OrderContext())
            {
                return context.Orders.ToList();
            }
        }

       
    }

}
   