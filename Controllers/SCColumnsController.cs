using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TestBaseCopy.Dtos;
using TestBaseCopy.Helpers;

namespace TestBaseCopy.Controllers
{
    public class SCColumnsController : BaseApiController
    {
        private readonly DatabaseHelper _databaseHelper;
        public SCColumnsController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [HttpGet("SCColumnsSelect")]
        public async Task<ActionResult<IEnumerable<SCColumnsSelectDto>>> GetSCColumnsSelect()
        {
            var databaseName = "ScannerTest";

            // Call the stored procedure and get the results as ScColumns
            var SCColumnsSelect = await _databaseHelper.ExecuteStoredProcedureAsync<SCColumnsSelectDto>("SC_Columns_Select", databaseName, isQuery: true);

            return Ok(SCColumnsSelect);
        }
    }
}