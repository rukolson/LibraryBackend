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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace LibraryBackend.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly IWebHostEnvironment _env;
		public BookController(IConfiguration configuration, IWebHostEnvironment env)
		{
			_configuration = configuration;
			_env = env;
		}


		[HttpGet]
		public JsonResult Get()
		{
			string query = @"
                            select BookId, BookName, Category,
                            convert(varchar(10),DateOfAdding,120) as DateOfAdding, CoverPicture
                            from
                            dbo.Book
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
		public JsonResult Post(Book emp)
		{
			string query = @"
                           insert into dbo.Book
                           (BookName,Category,DateOfAdding,CoverPicture)
                    values (@BookName,@Category,@DateOfAdding,@CoverPicture)
                            ";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@BookName", emp.BookName);
					myCommand.Parameters.AddWithValue("@Category", emp.Category);
					myCommand.Parameters.AddWithValue("@DateOfAdding", emp.DateOfAdding);
					myCommand.Parameters.AddWithValue("@CoverPicture", emp.CoverPicture);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult("Dodano prawidłowo");
		}


		[HttpPut]
		public JsonResult Put(Book emp)
		{
			string query = @"
                           update dbo.Book
                           set BookName= @BookName,
                            Category=@Category,
                            DateOfAdding=@DateOfAdding,
                            CoverPicture=@CoverPicture
                            where BookId=@BookId
                            ";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@BookId", emp.BookId);
					myCommand.Parameters.AddWithValue("@BookName", emp.BookName);
					myCommand.Parameters.AddWithValue("@Category", emp.Category);
					myCommand.Parameters.AddWithValue("@DateOfAdding", emp.DateOfAdding);
					myCommand.Parameters.AddWithValue("@CoverPicture", emp.CoverPicture);
					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult("Zaktualizowano prawidłowo");
		}

		[HttpDelete("{id}")]
		public JsonResult Delete(int id)
		{
			string query = @"
                           delete from dbo.Book
                            where BookId=@BookId
                            ";

			DataTable table = new DataTable();
			string sqlDataSource = _configuration.GetConnectionString("DefaultConnection");
			SqlDataReader myReader;
			using (SqlConnection myCon = new SqlConnection(sqlDataSource))
			{
				myCon.Open();
				using (SqlCommand myCommand = new SqlCommand(query, myCon))
				{
					myCommand.Parameters.AddWithValue("@BookId", id);

					myReader = myCommand.ExecuteReader();
					table.Load(myReader);
					myReader.Close();
					myCon.Close();
				}
			}

			return new JsonResult("Usunięto prawidłowo");
		}


		[Route("SaveFile")]
		[HttpPost]
		public JsonResult SaveFile()
		{
			try
			{
				var httpRequest = Request.Form;
				var postedFile = httpRequest.Files[0];
				string filename = postedFile.FileName;
				var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

				using (var stream = new FileStream(physicalPath, FileMode.Create))
				{
					postedFile.CopyTo(stream);
				}

				return new JsonResult(filename);
			}
			catch (Exception)
			{

				return new JsonResult("anonymous.png");
			}
		}

	}
}