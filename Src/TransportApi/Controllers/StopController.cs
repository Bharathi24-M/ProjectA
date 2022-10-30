using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using TransportApi.Models;

namespace TransportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StopController : ControllerBase
    {
        private readonly TransportContext db;

        public StopController(TransportContext _db)
        {
            db = _db;

        }
        [HttpGet]
        public IActionResult GetStopInfo()
        {

            return Ok(db.StopInfos.ToList());
        }
        [HttpPost]
        public IActionResult AddStopInfo(StopInfo e)
        {
            db.StopInfos.Add(e);
            db.SaveChanges();
            return Ok(e);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetStopbyId(int id)
        {
            var result = db.StopInfos.Find(id);
            return Ok(result);
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult EditStop(int id, StopInfo e)
        {
            db.StopInfos.Update(e);
            db.SaveChanges();
            return Ok(e);
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteStop(int id)
        {
            try
            {
                var result = db.StopInfos.Find(id);
                db.StopInfos.Remove(result);
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
