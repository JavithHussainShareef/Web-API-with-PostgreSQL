using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Data;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public DepartmentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public JsonResult Get()
        
        {
            string query = @"select DepartmentId as ""DepartmentId"",DepartmentName as ""DepartmentName"" from Department";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using(NpgsqlCommand mycommand = new NpgsqlCommand(query,mycon))
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
        public JsonResult Post(Models.Department dep)

        {
            // string query = @"insert into Department(DepartmentName) values (@DepartmentName)";
            string query = @"call insert_row_employee(@DepartmentId,@DepartmentName)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand mycommand = new NpgsqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
                    mycommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }
        [HttpPut]
        public JsonResult Put(Models.Department dep)

        {
            string query = @"update Department set DepartmentName=@DepartmentName where DepartmentId=@DepartmentId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand mycommand = new NpgsqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@DepartmentName", dep.DepartmentName);
                    mycommand.Parameters.AddWithValue("@DepartmentId", dep.DepartmentId);
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
            string query = @"delete from  Department where DepartmentId=@DepartmentId";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("EmployeeConnection");
            NpgsqlDataReader myReader;
            using (NpgsqlConnection mycon = new NpgsqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (NpgsqlCommand mycommand = new NpgsqlCommand(query, mycon))
                {
                    mycommand.Parameters.AddWithValue("@DepartmentId",id);
                    
                    myReader = mycommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }

    }
}
