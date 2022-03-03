using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public EmployeeController(IConfiguration configuration,IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [HttpGet]
        public JsonResult Get()

        {
            string query = @"select EmployeeId as ""EmployeeId"",EmployeeName as ""EmployeeName"",Department as ""Department"", to_char(DateOfJoining,'YYYY-MM-DD') as ""DateOfJoining"",PhotoFileName as ""PhotoFileName"" from Employee";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand mycommand = new NpgsqlCommand(query, mycon))
                {
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);
        }
        [HttpPost]
        public JsonResult Post(Models.Employee emp)

        {
            string query = @"insert into Employee(EmployeeName,Department,DateOfJoining,PhotoFileName) values (@EmployeeName,@Department,@DateOfJoining,@PhotoFileName)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand mycommand = new NpgsqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    mycommand.Parameters.AddWithValue("@Department", emp.Department);
                    mycommand.Parameters.AddWithValue("@DateOfJoining", Convert.ToDateTime(emp.DateofJoining));
                    mycommand.Parameters.AddWithValue("@PhotoFileName", emp.PhotoFileName);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }
        [HttpPut]
        public JsonResult Put(Models.Employee emp)

        {
            string query = @"update Employee set EmployeeName=@EmployeeName,Department=@Department,DateOfJoining=@DateOfJoining,photofilename=@photofilename where EmployeeId=@EmployeeId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand mycommand = new NpgsqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
                    mycommand.Parameters.AddWithValue("@EmployeeName", emp.EmployeeName);
                    mycommand.Parameters.AddWithValue("@Department", emp.Department);
                    mycommand.Parameters.AddWithValue("@DateOfJoining", Convert.ToDateTime(emp.DateofJoining));
                    mycommand.Parameters.AddWithValue("@photofilename", emp.PhotoFileName);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }
        [HttpDelete("{id}")]
        public JsonResult Delete(int id)

        {
            string query = @"delete from  Employee where EmployeeID=@EmployeeID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand mycommand = new NpgsqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@EmployeeID", id);

                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }
        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string fileName = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/photos/" + fileName;
                using(var stream = new FileStream(physicalPath,FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }
                return new JsonResult(fileName);
            }
            catch
            {
                return new JsonResult("some went wrong in photos");
            }
        }
    }

}
