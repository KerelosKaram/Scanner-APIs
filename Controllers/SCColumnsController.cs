using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Scanner.Dtos;
using TestBaseCopy.Helpers;

namespace TestBaseCopy.Controllers
{
    public class ScannerController : BaseApiController
    {
        private readonly DatabaseHelper _databaseHelper;
        private readonly string databaseName = "ScannerTest";
        public ScannerController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [HttpGet("SCColumnsSelect")]
        public async Task<ActionResult<IEnumerable<SCColumnsSelectDto>>> GetSCColumnsSelect()
        {
            // Call the stored procedure and get the results as ScColumns
            var SCColumnsSelect = 
                await _databaseHelper.ExecuteStoredProcedureAsync<SCColumnsSelectDto>("SC_Columns_Select", databaseName, isQuery: true);

            return Ok(SCColumnsSelect);
        }

        [HttpPost("SCColumnsInsert")]
        public async Task<ActionResult<SCColumnsInsertDto>> SCColumnsInsertDto([FromBody] SCColumnsInsertDto sCColumnsInsertDto)
        {

            await _databaseHelper.ExecuteStoredProcedureAsync<SCColumnsInsertDto>("SC_Columns_Insert", databaseName, sCColumnsInsertDto , isQuery: false);
            
            return Ok();
        }

    }
}