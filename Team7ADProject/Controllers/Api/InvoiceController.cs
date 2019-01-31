using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Team7ADProject.Entities;
using Team7ADProject.ViewModels.Api;

namespace Team7ADProject.Controllers.Api
{
    #region Gao Jiaxue
    public class InvoiceController : ApiController
    {
        private LogicDB _context;

        public InvoiceController()
        {
            _context = new LogicDB();
        }
        [Route("~/api/deliveryorder/select/{supplierid}")]
        public IHttpActionResult GetAllDO(string supplierid)
        {
            List<DeliveryOrder> deliveryOrders = _context.DeliveryOrder.Where(m => m.SupplierId ==supplierid && m.Status == "BILLED").ToList();
            if (deliveryOrders == null)
            {
                return NotFound();
            }
            List<DeliveryOrderDTO> viewModels = new List<DeliveryOrderDTO>();
            for (int i = 0; i < deliveryOrders.Count; i++)
            {
              DeliveryOrderDTO viewModel = new DeliveryOrderDTO();
                viewModel.DelOrderId = deliveryOrders[i].DelOrderId;
                viewModel.DelOrderNo = deliveryOrders[i].DelOrderNo;
                viewModels.Add(viewModel);
            }
            return Ok(viewModels);
        }

    }
    #endregion
}
