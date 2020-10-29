using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using RestSharp;
using Portal_Item;
using RestSharp.Authenticators;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace Captive_Portal.Controllers
{ 
    [ApiController]
    [Route("api/[controller]")]
    public class CaptivaPortalController : ControllerBase
    {
        private readonly IConfiguration _config;
        private static List<PortalItem> Portal = new List<PortalItem> {
            new PortalItem
            {
                Name= string.Empty,
                Email= string.Empty,

            }
        };

        public CaptivaPortalController(IConfiguration config)
        {
            _config = config;

        }
/// <summary>
/// 
/// </summary>
/// <param name="name"></param>
/// <returns></returns>
       [HttpGet]
       [Route("lookup/{name}")]
        public ActionResult<PortalItem> Get(string name)
        {
            var portalitem = Portal.Find(item =>
                item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (portalitem == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(portalitem);
            }
        }

        [HttpGet]
        [Route("getToDo/{id}")]
        public ActionResult<string> toDo(string id)
        {
            var client = new RestClient($"https://jsonplaceholder.typicode.com/todos/{id}");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return response.Content;
            }
            return BadRequest("Site can't be reached");
        }

        [HttpGet]
        [Route("getWeather/{cityName}")]
        public ActionResult<string> getWeather(string cityName)
        {
            var key = _config.GetValue<string>("AppSettings:apiKey");
            var client = new RestClient($"http://api.openweathermap.org/data/2.5/weather?q={cityName}&appid={key}");
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

           


            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Content;
            }
            return BadRequest("Site can't be reached");
        }

         

        [HttpPost]
        [Route("add/{name}")]
        public ActionResult Post(PortalItem portalitem)
        {
            var existingPortalItem = Portal.Find(item =>
                item.Name.Equals(portalitem.Name, StringComparison.InvariantCultureIgnoreCase));

            if (existingPortalItem != null)
            {
                return Conflict("Cannot create the user because it already exists.");
            }
            else
            {
                Portal.Add(portalitem);
                var resourceUrl = Path.Combine(Request.Path.ToString(), Uri.EscapeUriString(portalitem.Name));
                return Created(resourceUrl, portalitem);
            }
        }
        [HttpPut]
        [Route("update/{name}")]
        public ActionResult Put(PortalItem portalitem)
        {
            var existingPortalItem = Portal.Find(item =>
                item.Name.Equals(portalitem.Name, StringComparison.InvariantCultureIgnoreCase));
            if (existingPortalItem == null)
            {
                return BadRequest("Cannot update a nonexisting user.");
            }
            else
            {
                existingPortalItem.Email = portalitem.Email;
                return Ok();
            }
        }

        [HttpPut]
        [Route("updatePhone/{id}/{phoneNumber}")]
        public ActionResult UpdatePhoneNumber(PortalItem portalitem)
        {
            var existingPortalItem = Portal.Find(item =>
                item.Name.Equals(portalitem.Name, StringComparison.InvariantCultureIgnoreCase));
            if (existingPortalItem == null)
            {
                return BadRequest("Cannot update a nonexisting user.");
            }
            else
            {
                existingPortalItem.Email = portalitem.Email;
                return Ok();
            }
        }

        [HttpDelete]
        [Route("{name}")]
        public ActionResult Delete(string term)
        {
            var portalitem = Portal.Find(item =>
                item.Name.Equals(term, StringComparison.InvariantCultureIgnoreCase));

            if (portalitem == null)
            {
                return NotFound();
            }
            else
            {
                Portal.Remove(portalitem);
                return NoContent();
            }
        }

        
    }
    
}