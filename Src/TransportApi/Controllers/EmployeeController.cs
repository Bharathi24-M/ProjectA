using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TransportApi.Models;

namespace TransportApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly TransportContext db;

        public EmployeeController(TransportContext _db)
        {
            db = _db;

        }
        [HttpGet]
        public IActionResult GetEmployee()
        {
            return Ok(db.EmployeeInfos.ToList());
        }
        [HttpPost]
        public IActionResult AddEmployee(EmployeeInfo e)
        {
            db.EmployeeInfos.Add(e);
            db.SaveChanges();
            return Ok(e);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetEmpbyId(int id)
        {
            var result = db.EmployeeInfos.Find(id);
            return Ok(result);
        }
        [HttpPut]
        [Route("{id}")]
        public IActionResult EditEmployee(int id, EmployeeInfo e)
        {
            db.EmployeeInfos.Update(e);
            db.SaveChanges();
            return Ok(e);
        }
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var result = db.EmployeeInfos.Find(id);            
            db.EmployeeInfos.Remove(result);
            db.SaveChanges();
            return Ok(result);

        }
    }
}
