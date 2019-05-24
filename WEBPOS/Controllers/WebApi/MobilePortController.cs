using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using WEBPOS.DataAccess.BusinessLayer;
using System.Threading.Tasks;
using WEBPOS.DataAccess.DataEntities;
using WEBPOS.DataAccess.DataLayer;
using System;
using WEBPOS.Models;
using AutoMapper;

namespace WEBPOS.Controllers.WebApi
{
    public class MobilePortController : ApiController
    {
        [HttpPost]
        [Route("mob/save_sell_order")]
        public async Task<IHttpActionResult> SaveSellOrder([FromUri] string password, [FromUri] string userCode)
        {
            if (password == BlTable.Read(new DeTable { KeyFixed = "ServerPassword" }).FirstOrDefault().KeyVariable)
            {
                try
                {
                    var jsonObject = await Request.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<Tuple<SellOrderModel, List<SellOrderDetailModel>>>(jsonObject); 

                    var head = model.Item1;
                    var detail = model.Item2;

                    head.SellOrderId = 0;
                    
                    var objHead = Mapper.Map<DeSellOrder>(head);
                    objHead.UpdateUser = userCode;
                    BlSellOrder.Save(objHead);

                    foreach (var item in detail)
                    {
                        item.SellOrderId = objHead.SellOrderId;
                        item.VatCode = BlTax.ReadByValue(item.VatPercent).TaxCode;
                        item.WarehouseCode = BlWarehouse.ReadAllQueryable().FirstOrDefault().WarehouseCode;
                        var obj = Mapper.Map<DeSellOrderDetail>(item);
                        obj.UpdateUser = userCode;
                        BlSellOrderDetail.Save(obj);
                    }
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Incorrect Password");
            }
            return Ok("Ok-Success");
        }

        [HttpPost]
        [Route("mob/update_sell_order")]
        public async Task<IHttpActionResult> UpdateSellOrder([FromUri] string password, [FromUri] string userCode)
        {
            if (password == BlTable.Read(new DeTable { KeyFixed = "ServerPassword" }).FirstOrDefault().KeyVariable)
            {
                try
                {
                    var jsonObject = await Request.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<Tuple<SellOrderModel, List<SellOrderDetailModel>>>(jsonObject);

                    var head = model.Item1;
                    var detail = model.Item2;

                    var objHead = Mapper.Map<DeSellOrder>(head);
                    objHead.UpdateUser = userCode;
                    //BlSellOrder.Save(objHead);

                    foreach (var item in BlSellOrderDetail.ReadAllQueryable().Where(x=>x.SellOrderId == objHead.SellOrderId))
                    {
                        BlSellOrderDetail.Delete(item );
                    }

                    foreach (var item in detail)
                    {
                        item.SellOrderId = objHead.SellOrderId;
                        item.VatCode = BlTax.ReadByValue(item.VatPercent).TaxCode;
                        item.WarehouseCode = BlWarehouse.ReadAllQueryable().FirstOrDefault().WarehouseCode;
                        var obj = Mapper.Map<DeSellOrderDetail>(item);
                        obj.UpdateUser = userCode;
                        BlSellOrderDetail.Save(obj);
                    }

                    var aOrders = BlUserSellOrder.ReadAllQueryable().Where(x => x.UserCode == userCode && x.SellOrderId == objHead.SellOrderId);

                    foreach (var order in aOrders)
                    {
                        order.UserOrderState = UserOrderState.CONCLUIDO;
                        BlUserSellOrder.Save(order);
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Incorrect Password");
            }
            return Ok("Ok-Success");
        }

        [HttpGet]
        [Route("mob/test")]
        public IHttpActionResult Get([FromUri] string password)
        {
            var res = new Response();
            if (password == BlTable.Read(new DeTable { KeyFixed = "ServerPassword" }).FirstOrDefault().KeyVariable)
            {
                res = new Response
                {
                    IsSuccess = true,
                    Message = RequestStateMessage.SUCCESS.ToString(),
                    ResponseData = null
                };
            }
            else
            {
                res = new Response
                {
                    IsSuccess = false,
                    Message = RequestStateMessage.FAILURE.ToString(),
                    ResponseData = null
                };
            }

            return Ok(res);
        }

        [HttpGet]
        [Route("mob/items")]
        public IHttpActionResult GetItems([FromUri] string password)
        {
            var res = new Response();
            if (password == BlTable.Read(new DeTable { KeyFixed = "ServerPassword" }).FirstOrDefault().KeyVariable)
            {
                res = new Response
                {
                    IsSuccess = true,
                    Message = RequestStateMessage.SUCCESS.ToString(),
                    ResponseData = JsonConvert.SerializeObject(BlItem.ReadAllQueryable().Select(x => new
                    {
                        x.ItemCode,
                        x.ItemDescription,
                        x.Barcode,
                        TaxValue = BlTax.ReadAllQueryable().FirstOrDefault(m => m.TaxCode == x.TaxCode)?.TaxPercent ?? 0,
                        x.DepartmentCode
                    })).ToString()
                };
            }
            else
            {
                res = new Response
                {
                    IsSuccess = false,
                    Message = RequestStateMessage.FAILURE.ToString(),
                    ResponseData = null
                };
            }

            return Ok(res);
        }

        [HttpGet]
        [Route("mob/prices")]
        public IHttpActionResult GetPrices([FromUri] string password)
        {
            var res = new Response();
            if (password == BlTable.Read(new DeTable { KeyFixed = "ServerPassword" }).FirstOrDefault().KeyVariable)
            {
                res = new Response
                {
                    IsSuccess = true,
                    Message = RequestStateMessage.SUCCESS.ToString(),
                    ResponseData = JsonConvert.SerializeObject(BlPrice.ReadAllQueryable().Select(x => new
                    {
                        x.ItemCode,
                        x.PriceListCode,
                        x.SellPrice
                    })).ToString()
                };
            }
            else
            {
                res = new Response
                {
                    IsSuccess = false,
                    Message = RequestStateMessage.FAILURE.ToString(),
                    ResponseData = null
                };
            }

            return Ok(res);
        }

        [HttpGet]
        [Route("mob/pricelists")]
        public IHttpActionResult GetPriceLists([FromUri] string password)
        {
            var res = new Response();
            if (password == BlTable.Read(new DeTable { KeyFixed = "ServerPassword" }).FirstOrDefault().KeyVariable)
            {
                res = new Response
                {
                    IsSuccess = true,
                    Message = RequestStateMessage.SUCCESS.ToString(),
                    ResponseData = JsonConvert.SerializeObject(BlPriceList.ReadAllQueryable().Select(x => new
                    {
                        x.PriceListCode,
                        x.PriceListDescription
                    })).ToString()
                };
            }
            else
            {
                res = new Response
                {
                    IsSuccess = false,
                    Message = RequestStateMessage.FAILURE.ToString(),
                    ResponseData = null
                };
            }

            return Ok(res);
        }

        [HttpGet]
        [Route("mob/customers")]
        public IHttpActionResult GetCustomers([FromUri] string password)
        {
            var res = new Response();
            if (password == BlTable.Read(new DeTable { KeyFixed = "ServerPassword" }).FirstOrDefault().KeyVariable)
            {
                res = new Response
                {
                    IsSuccess = true,
                    Message = RequestStateMessage.SUCCESS.ToString(),
                    ResponseData = JsonConvert.SerializeObject(BlBusinessPartner.ReadAllQueryable().Where(x => x.BusinessPartnerType == "C").Select(x => new
                    {
                        x.BusinessPartnerCode,
                        x.BusinessPartnerDescription,
                        x.RNC,
                        x.PriceListCode
                    })).ToString()
                };
            }
            else
            {
                res = new Response
                {
                    IsSuccess = false,
                    Message = RequestStateMessage.FAILURE.ToString(),
                    ResponseData = null
                };
            }

            return Ok(res);
        }

        [HttpGet]
        [Route("mob/orders")]
        public IHttpActionResult GetOrders([FromUri] string password, [FromUri] string userCode)
        {
            var res = new Response();
            if (password == BlTable.Read(new DeTable { KeyFixed = "ServerPassword" }).FirstOrDefault().KeyVariable)
            {
                var aOrders = BlUserSellOrder.ReadAllQueryable().Where(x => x.UserCode == userCode && x.UserOrderState == UserOrderState.ASIGNADA);
                var orderIds = aOrders.Select(x => x.SellOrderId);

                foreach (var order in aOrders)
                {
                    order.UserOrderState = UserOrderState.TRABAJANDO;
                    BlUserSellOrder.Save(order);
                }

                var orders = BlSellOrder.ReadAllQueryable().Where(x => orderIds.Contains(x.SellOrderId));
                var headList = new List<SellOrderModel>();
                var detailList = new List<SellOrderDetailModel>();
                foreach (var item in orders)
                {
                    var obj = Mapper.Map<SellOrderModel>(item);
                    headList.Add(obj);

                    foreach (var detail in BlSellOrderDetail.ReadAllQueryable().Where(x => x.SellOrderId == item.SellOrderId))
                    {
                        var objDetail = Mapper.Map<SellOrderDetailModel>(detail);
                        detailList.Add(objDetail);
                    }
                }

                var model = new Tuple<List<SellOrderModel>, List<SellOrderDetailModel>>(headList, detailList);
                res = new Response
                {
                    IsSuccess = true,
                    Message = RequestStateMessage.SUCCESS.ToString(),
                    ResponseData = JsonConvert.SerializeObject(model).ToString()
                };
            }
            else
            {
                res = new Response
                {
                    IsSuccess = false,
                    Message = RequestStateMessage.FAILURE.ToString(),
                    ResponseData = null
                };
            }

            return Ok(res);
        }

        [HttpGet]
        [Route("mob/authenticateUser")]
        public IHttpActionResult AuthenticateUser([FromUri] string password, string userCode, string userPassword)
        {
            var res = new Response();
            if (password == BlTable.Read(new DeTable { KeyFixed = "ServerPassword" }).FirstOrDefault().KeyVariable)
            {
                var user = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserType != DataAccess.DataEntities.UserType.CAJERO && x.UserCode == userCode);
                res = new Response { IsSuccess = false, Message = "Usuario o contraseña incorrectos", ResponseData = null };

                if (user == null)
                    return Ok(res);

                var userMobileProfile = BlUserMobileProfile.ReadAllQueryable().FirstOrDefault(x => x.UserCode == user.UserCode);

                if (userMobileProfile == null)
                {
                    res.Message = "El usuario no esta permitido usar esta aplicacion.";
                    return Ok(res);
                }
                if (userPassword == BlUser.DecryptString(user.Password, userCode))
                    res = new Response
                    {
                        IsSuccess = true,
                        Message = RequestStateMessage.SUCCESS.ToString(),
                        ResponseData = JsonConvert.SerializeObject(new
                        {
                            UserCode = user.UserCode,
                            Password = userPassword,
                            user.Name,
                            user.LastName,
                            user.Gender,
                            userMobileProfile.MobileProfileType
                        }).ToString()
                    };
            }
            else
                res = new Response { IsSuccess = false, Message = RequestStateMessage.FAILURE.ToString(), ResponseData = null };

            return Ok(res);
        }
    }

    public class Response
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string ResponseData { get; set; }
    }

    enum RequestStateMessage
    {
        SUCCESS = 0,
        FAILURE = 1
    }
}
