using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using WEBPOS.DataAccess.BusinessLayer;

namespace WEBPOS.Controllers.WebApi
{
    public class MobilePortController : ApiController
    {
        private string _password = "CentraPass"; 
        [HttpGet]
        [Route("mob/test")]
        public IHttpActionResult Get([FromUri] string password)
        {
            var res = new Response();
            if (password == _password)
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
        public IHttpActionResult GetItems()
        {
            var res = new Response
            {
                IsSuccess = true,
                Message = RequestStateMessage.SUCCESS.ToString(),
                ResponseData = JsonConvert.SerializeObject(BlItem.ReadAllQueryable().Select(x => new
                {
                    x.ItemCode,
                    x.ItemDescription,
                    x.Barcode,
                    TaxValue = BlTax.ReadAllQueryable().FirstOrDefault(m=>m.TaxCode == x.TaxCode)?.TaxPercent ?? 0,
                    x.DepartmentCode
                })).ToString()
            };

            return Ok(res);
        }

        [HttpGet]
        [Route("mob/prices")]
        public IHttpActionResult GetPrices([FromUri] string password)
        {
            var res = new Response();
            if (password == _password)
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
            if (password == _password)
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
            if (password == _password)
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
        [Route("mob/authenticateUser")]
        public IHttpActionResult AuthenticateUser([FromUri] string password, string userCode, string userPassword)
        {
            var res = new Response();
            if (password == _password)
            {
                var user = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserType == DataAccess.DataEntities.UserType.MOVIL && x.UserCode == userCode);
                res = new Response { IsSuccess = false, Message = RequestStateMessage.FAILURE.ToString(), ResponseData = null };

                if (user == null)
                    return Ok(res);

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
                            user.Gender
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
