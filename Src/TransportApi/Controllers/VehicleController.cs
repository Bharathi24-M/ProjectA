using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TransportApi.Models;

namespace TransportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly TransportContext db;
        public VehicleController(TransportContext _db)
        {
            db = _db;

        }
        [HttpGet]
        public IActionResult GetVehicleInfo()
        {
            return Ok(db.VehicleInfos.ToList());
        }

        [HttpPost]
        public IActionResult AddVehicleInfo(VehicleInfo e)
        {
            var result = db.VehicleInfos.Where(x => x.VehicleNum == e.VehicleNum && x.RouteNum == e.RouteNum).Count();
            if (result > 0)
            {
                return Conflict();
            }
            db.VehicleInfos.Add(e);
            db.SaveChanges();
            return Ok(e);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetVehiclebyId(int id)
        {
            var result = db.VehicleInfos.Where(x => x.VehicleId == id).SingleOrDefault();
            return Ok(result);
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult EditVehicle(int id, VehicleInfo e)
        {
            var result = db.VehicleInfos.Where(x => x.VehicleNum == e.VehicleNum && x.RouteNum == e.RouteNum && x.VehicleId != e.VehicleId).Count();
            if (result > 0)
            {
                return Conflict();
            }
            db.VehicleInfos.Update(e);
            db.SaveChanges();
            return Ok(e);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteVehicle(int id)
        {

            try
            {
                var result = db.VehicleInfos.Where(x => x.VehicleId == id).SingleOrDefault();
                if (result != null)
                {
                    db.VehicleInfos.Remove(result);
                    db.SaveChanges();
                }
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
