using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scanner.Dtos;
using Scanner.Helpers;

namespace Scanner.Controllers
{
    public class ScannerController : BaseApiController
    {
        private readonly DatabaseHelper _databaseHelper;

        public ScannerController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [HttpGet("SCColumnsSelect")]
        public async Task<ActionResult<List<SCColumnsSelectDto>>> SCColumnsSelect()
        {
            // Call the stored procedure and get the results as SCColumnsSelect
            var SCColumnsSelect = 
                await _databaseHelper
                    .ExecuteStoredProcedureAsync<SCColumnsSelectDto>
                    ("SC_Columns_Select", ".", "ScannerTest", "sa", "123456", isQuery: true);

            return Ok(SCColumnsSelect);
        }


        [HttpGet("SCCustomersSelect")]
        public async Task<ActionResult<List<SCCustomersSelectDto>>> SCCustomersSelect([FromQuery]string SCUserName)
        {
            var parameters = new { SCUserName };
            var SCCustomersSelect = 
                await _databaseHelper
                    .ExecuteStoredProcedureAsync<SCCustomersSelectDto>
                    ("SCCustomersSelect", ".", "ScannerTest", "sa", "123456", parameters, isQuery: true);

            return Ok(SCCustomersSelect);
        }


        [HttpGet("SCCustomerSwap")]
        public async Task<ActionResult<bool>> SCCustomerSwap([FromQuery] string index, [FromQuery] string New_Index)
        {
            var parameters = new { index, New_Index };
            var SCCustomerSwap = 
                await _databaseHelper
                    .ExecuteStoredProcedureAsync<bool>
                    ("Sc_Cus_index2", ".", "ScannerTest", "sa", "123456", parameters, isQuery: false);

            return Ok(SCCustomerSwap);
        }


        // Should be POST Method but due to the Android code.
        [HttpGet("SCNewCustomerInsert")]
        public async Task<ActionResult<bool>> SCNewCustomerInsert(
            [FromQuery] string Customer_Name, [FromQuery(Name = "long")] string _long, [FromQuery] string lat, [FromQuery] int New_Index)
        {
            var parameters = new 
            { 
                Customer_Name,
                Long = _long,
                lat,
                New_Index,
            };

            var SCNewCustomerInsert =
                await _databaseHelper
                    .ExecuteStoredProcedureAsync<bool>
                    ("Sc_Cus_index", ".", "ScannerTest", "sa", "123456", parameters, isQuery: false);

            return Ok(SCNewCustomerInsert);
        }


        [HttpGet("SCVisitDetailsSelect")]
        public async Task<ActionResult<List<VisitDetailsSelectDto>>> SCVisitDetailsSelect([FromQuery] int SC_Customer_ID)
        {
            var parameters = new { SC_Customer_ID };
            var SCVisitDetailsSelect = 
                await _databaseHelper
                    .ExecuteStoredProcedureAsync<VisitDetailsSelectDto>
                    ("Select_SC_Visit_Details", ".", "ScannerTest", "sa", "123456", parameters, isQuery: true);

            return Ok(SCVisitDetailsSelect);
        }


        [HttpGet("SCVisitPicInsert")]
        public async Task<ActionResult<bool>> SCVisitPicSelect(
            [FromQuery] int SC_Visit_header_ID,
            [FromQuery] string Pic_Name,
            [FromQuery] string Insert_User)
        {
            var parameters = new { SC_Visit_header_ID , Pic_Name , Insert_User };
            var SCVisitPicInsert = 
                await _databaseHelper
                    .ExecuteStoredProcedureAsync<bool>
                    ("SC_Visit_Pic_Insert", ".", "ScannerTest", "sa", "123456", parameters, isQuery: false);

            return Ok(SCVisitPicInsert);
        }        


        [HttpGet("SCVisitPicSelect")]
        public async Task<ActionResult<List<SCVisitPicSelectDto>>> SCVisitPicSelect([FromQuery] int SC_Customer_ID)
        {
            var parameters = new { SC_Customer_ID };
            var SCVisitPicSelect = 
                await _databaseHelper
                    .ExecuteStoredProcedureAsync<SCVisitPicSelectDto>
                    ("Select_SC_Visit_Pic", ".", "ScannerTest", "sa", "123456", parameters, isQuery: true);

            return Ok(SCVisitPicSelect);
        }


        [HttpGet("SCVisitHeaderInsert")]
        public async Task<ActionResult<List<SCVisitHeaderInsertDto>>> SCVisitHeaderInsert(
            [FromQuery] int SC_Customer_ID, 
            [FromQuery] string Scanner_Name,
            [FromQuery] DateTime Start_Date,
            [FromQuery] DateTime End_Date,
            [FromQuery] string Long,
            [FromQuery] string lat,
            [FromQuery] string Insert_User)
        {
            var parameters = new { SC_Customer_ID, Scanner_Name, Start_Date, End_Date, Long, lat, Insert_User };
            var SCVisitHeaderInsert = 
                await _databaseHelper
                    .ExecuteStoredProcedureAsync<SCVisitHeaderInsertDto>
                    ("SC_Visit_header_Insert", ".", "ScannerTest", "sa", "123456", parameters, isQuery: false);

            return Ok(SCVisitHeaderInsert);
        }


        [HttpGet("SCVisitDetailsInsert")]
        public async Task<ActionResult<List<bool>>> SCVisitDetailsInsert(
            [FromQuery] int SC_Visit_header_ID, 
            [FromQuery] int SC_Column_ID,
            [FromQuery] string Result,
            [FromQuery] string Insert_User)
        {
            var parameters = new { SC_Visit_header_ID, SC_Column_ID, Result, Insert_User };
            var SCVisitDetailsInsert = 
                await _databaseHelper
                    .ExecuteStoredProcedureAsync<bool>
                    ("SC_Visit_Details_Insert", ".", "ScannerTest", "sa", "123456", parameters, isQuery: false);

            return Ok(SCVisitDetailsInsert);
        }

        [HttpPost("docfile")]
        public async Task<ActionResult<List<object>>> docfile()
        {
             if (Request.Form.Files.Count > 0)
            {
                var docfiles = new List<string>();

                foreach (var file in Request.Form.Files)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Upload", "Scanner", file.FileName);

                    
                    // Ensure directory exists
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    // Save the file to the specified path
                    await using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    docfiles.Add(filePath);
                }

                return StatusCode((int)HttpStatusCode.Created, docfiles);
            }

            return BadRequest("No files uploaded.");
        }

    }
}