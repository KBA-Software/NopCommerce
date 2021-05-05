using Nop.Core.Domain.Orders;
using Nop.Services.Events;
using Nop.Plugin.Misc.SAPIntegration.Services;
using Nop.Plugin.Misc.SAPIntegration.Models;
using Nop.Services.Customers;
using Nop.Services.Common;
using Nop.Core.Domain.Customers;
using Nop.Services.Orders;
using Nop.Services.Catalog;

namespace Nop.Plugin.Misc.SAPIntegration.Events
{
    public class OrderEventConsumer : IConsumer<OrderPlacedEvent>
    {
        #region Fields
        private readonly ISAPIntegrationService _sAPIntegrationService;
        private readonly ICustomerService _customerService;
        private readonly IAddressService _addressService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;
        #endregion

        #region Ctor
        public OrderEventConsumer(ISAPIntegrationService sAPIntegrationService,
            ICustomerService customerService,
            IAddressService addressService,
            IGenericAttributeService genericAttributeService,
            IOrderService orderService,
            IProductService productService)
        {
            _sAPIntegrationService = sAPIntegrationService;
            _customerService = customerService;
            _addressService = addressService;
            _genericAttributeService = genericAttributeService;
            _orderService = orderService;
            _productService = productService;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Handle the order placed event
        /// </summary>
        /// <param name="eventMessage">The event message.</param>
        public void HandleEvent(OrderPlacedEvent eventMessage)
        {
            var order = eventMessage.Order;

            var customer = _customerService.GetCustomerById(order.CustomerId);
            var shipAddress = _addressService.GetAddressById((int)order.ShippingAddressId);

            var model = new SAPOrderModel()
            {
                OrderNumber = order.Id,
                CustFName = _genericAttributeService.GetAttribute<string>(customer, NopCustomerDefaults.FirstNameAttribute),
                CustLName = _genericAttributeService.GetAttribute<string>(customer, NopCustomerDefaults.LastNameAttribute),
                ShipAddress = shipAddress?.Address1 + "," + shipAddress?.Address2,
                ShipCity = shipAddress ?.City,
                ShippingStatusId = order.ShippingStatusId,
                OrderStatusId = order.OrderStatusId,
                PaymentMetod = order.PaymentMethodSystemName,
                OrderSubtotalInclTax = order.OrderSubtotalInclTax,
                OrderSubtotalExclTax = order.OrderSubtotalExclTax,
                OrderTotal = order.OrderTotal,
                ShippingMethod = order.ShippingMethod

            };
            var orderItem = _orderService.GetOrderItems(order.Id);
            foreach (var item in orderItem)
            {
                var product = _productService.GetProductById(item.ProductId);
                model.OrderItem.Add(new SAPOrderItemModel()
                {
                    OrderNumber = order.Id,
                    ProductName = product?.Name,
                    Quantity = item.Quantity,
                    Price = item.UnitPriceInclTax,
                    ItemWeight = (decimal)item.ItemWeight
                });
            }

            _sAPIntegrationService.SyncOrderToSAP(model);
        }
        #endregion
    }
}
