using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using LibraryBackend.Models;

namespace LibraryBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookCategoryController : ControllerBase
	{

		private readonly IConfiguration _configuration;
		public BookCategoryController(IConfiguration configuration)
		{
			_configuration = configuration;
		}


		[HttpGet]
		public JsonResult Get()
		{
			string query = @"
                            select CategoryId, CategoryName from
                            dbo.BookCategory
                            ";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult(table);
		}

		[HttpPost]
		public JsonResult Post(BookCategory dep)
		{
			string query = @"
                           insert into dbo.BookCategory
                           values (@CategoryName)
                            ";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@CategoryName", dep.CategoryName);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult("Added Successfully");
		}


		[HttpPut]
		public JsonResult Put(BookCategory dep)
		{
			string query = @"
                           update dbo.BookCategory
                           set CategoryName= @CategoryName
                            where CategoryId=@CategoryId
                            ";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@CategoryId", dep.CategoryId);
					myCommand.Parameters.AddWithValue("@CategoryName", dep.CategoryName);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult("Updated Successfully");
		}

		[HttpDelete("{id}")]
		public JsonResult Delete(int id)
		{
			string query = @"
                           delete from dbo.BookCategory
                            where CategoryId=@CategoryId
                            ";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@CategoryId", id);

					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult("Deleted Successfully");
		}


	}
}