using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TransportApi.Models;

namespace TransportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly TransportContext db;

        public RouteController(TransportContext _db)
        {
            db = _db;

        }
        [HttpGet]
        public IActionResult GetRouteInfo()
        {
            return Ok(db.RouteInfos.ToList());
        }
        [HttpPost]
        public IActionResult AddRouteInfo(RouteInfo e)
        {
            db.RouteInfos.Add(e);
            db.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetRoutebyId(int id)
        {
            var result = db.RouteInfos.Find(id);
            return Ok(result);
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult EditRoute(int id, RouteInfo e)
        {
            db.RouteInfos.Update(e);
            db.SaveChanges();
            return Ok(e);
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteRoute(int id)
        {
            try
            {
                var result = db.RouteInfos.Find(id);
                db.RouteInfos.Remove(result);
                db.SaveChanges();
                return Ok(result);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException e)
            {
                var sqlException = e.GetBaseException() as SqlException;
                if (sqlException != null)
                {
                    if (sqlException.Errors.Count > 0)
                    {
                        if (sqlException.Errors[0].Number == 547) // Exception Code 547 - Foreign Key violation
                        {
                            return StatusCode(424); //HTTP error code 424 - Failed Dependency
                        }
                    }
                }

            }
            return Ok();

        }
    }
}
