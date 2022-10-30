using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransportApi.Models;

namespace TransportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransportApiController : ControllerBase
    {
        private readonly TransportContext db;

        public TransportApiController(TransportContext _db)
        {
            db = _db;

        }
        [HttpGet]
        public IActionResult Login()
        {
            return Ok(db.AdminInfos.ToList());
        }
        [HttpPost]
        public IActionResult Login(AdminInfo AInfo)
        {
            try
            {
                var loginresult = (from item in db.AdminInfos
                                   where item.UserName == AInfo.UserName && item.Password == AInfo.Password
                                   select item).SingleOrDefault();
                if (loginresult != null)
                {
                    return Ok(loginresult);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception)
            {
                return Unauthorized();
            }


        }


    }
}
