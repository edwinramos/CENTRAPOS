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
        [HttpGet]
        [Route("mob/test")]
        public IHttpActionResult Get()
        {
            var res = BlUser.ReadAll();

            return Ok(res);
        }

        [HttpGet]
        [Route("mob/items")]
        public IHttpActionResult GetItems()
        {
            var response = BlItem.ReadAllQueryable().Select(x => new
            {
                x.ItemCode,
                x.ItemDescription,
                x.Barcode,
                x.TaxCode,
                x.DepartmentCode
            });

            return Ok(response);
        }

        [HttpGet]
        [Route("mob/prices")]
        public IHttpActionResult GetPrices()
        {
            var response = BlPrice.ReadAllQueryable().Select(x => new
            {
                x.ItemCode,
                x.PriceListCode,
                x.SellPrice
            });

            return Ok(response);
        }

        [HttpGet]
        [Route("mob/customers")]
        public IHttpActionResult GetCustomers()
        {
            var response = BlBusinessPartner.ReadAllQueryable().Where(x => x.BusinessPartnerType == "C").Select(x => new
            {
                x.BusinessPartnerCode,
                x.BusinessPartnerDescription,
                x.PriceListCode,
                x.PriceList.PriceListDescription
            });

            return Ok(response);
        }

        [HttpGet]
        [Route("mob/authenticateUser")]
        public IHttpActionResult AuthenticateUser(string userCode, string userPassword)
        {
            var user = BlUser.ReadAllQueryable().FirstOrDefault(x => x.UserType == DataAccess.DataEntities.UserType.MOVIL && x.UserCode == userCode);
            var res = new Response { IsSuccess = false, Message = RequestStateMessage.FAILURE.ToString(), ResponseData = null };

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
