using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using StockAPI.DtoModels;
using StockAPI.Entities;

namespace StockAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockValuesController : ControllerBase
    {
        private StocksContext _context2;
        private DockerStocksContext _context;
        //private IExtendedInterface _context;
        private masterContext _contextMaster;
        //private IDockerStocksContext _contextDocker;
        private DockerStocksContext _contextDocker;
        private DockerMasterContext _contextMasterDocker;
        //private Dictionary<string, IDockerStocksContext> _dbContextes;
        //private Dictionary<string, DbContext> _masterContextes;



        public StockValuesController(StocksContext context,masterContext masterContext, DockerStocksContext contextDocker, DockerMasterContext dockerMasterContext)
        {
            _context2 = context;
            _context = contextDocker;
            _contextMaster = masterContext;
            _contextDocker = contextDocker;
            //_contextDocker = contextDocker;
            _contextMasterDocker = dockerMasterContext;

            //_dbContextes = new Dictionary<string, IDockerStocksContext>()
            //{
            //    { "StockValues",_context},
            //    { "DockerStockValues",_contextDocker}
            //};

            //_masterContextes = new Dictionary<string, DbContext>()
            //{
            //    { "master",_context},
            //    { "DockerMaster",_contextDocker}
            //};
        }

        //[HttpPost("SwithContext")]
        //public async Task<ActionResult> SwitchContext([FromBody] DatabaseProperties dbProp)
        //{

        //    if (dbProp == null)
        //        return BadRequest("not specified db");

        //    if(!dbProp.DbContext.IsNullOrEmpty())
        //    {
        //        _context = _dbContextes[dbProp.DbContext] ?? _context;
        //    }

        //    return Ok();
        //}
        [HttpPost("RestoreDbFromSnapshot")]
        public async Task<ActionResult> RestoreDbFromSnapshot([FromBody] DatabaseProperties dbProp)
        {
            string query2 = @$"ALTER DATABASE {dbProp.DatabaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;";
            _contextMaster.Database.ExecuteSqlRaw(query2);

            string query = @$"RESTORE DATABASE {dbProp.DatabaseName} FROM DATABASE_SNAPSHOT = '{dbProp.SnapshotName}';";
            _contextMaster.Database.ExecuteSqlRaw(query);

            string query3 = @$"ALTER DATABASE {dbProp.DatabaseName} SET MULTI_USER;";
            _contextMaster.Database.ExecuteSqlRaw(query3);

            return Ok();
        }
        [HttpPost("CreateDatabase")]
        public async Task<ActionResult> CreateDatabase([FromBody] DatabaseProperties dbProp)
        {
            string query = @$"CREATE DATABASE {dbProp.DatabaseName} ;";
            _context.Database.ExecuteSqlRaw(query);
            return Ok();
        }
        // GET: api/StockValues
        [HttpPost("CreateSnapshot")]
        public async Task<ActionResult> CreateSnapshot([FromBody] DatabaseProperties dbProp)
        {
            //must be only one snapshot for database
            string query = @$"CREATE DATABASE {dbProp.SnapshotName} ON (NAME = '{dbProp.DatabaseName}', FILENAME = 'C:\\Path\\To\\{dbProp.SnapshotName}.ss') AS SNAPSHOT OF [{dbProp.DatabaseName}];";
            _context.Database.ExecuteSqlRaw(@$"CREATE DATABASE {dbProp.SnapshotName} ON (NAME = '{dbProp.DatabaseName}', FILENAME = 'C:\\Path\\To\\{dbProp.SnapshotName}.ss') AS SNAPSHOT OF [{dbProp.DatabaseName}];");
            return Ok();
        }
        // GET: api/StockValues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StockValues>>> GetStockValues()
        {
          if (_context.StockValues == null)
          {
              return NotFound();
          }
            return await _context.StockValues.ToListAsync();
        }

        // GET: api/StockValues/5
        [HttpGet("{id}")]
        public async Task<ActionResult<StockValues>> GetStockValues(int id)
        {
          if (_context.StockValues == null)
          {
              return NotFound();
          }
            var stockValues = await _context.StockValues.FindAsync(id);

            if (stockValues == null)
            {
                return NotFound();
            }

            return stockValues;
        }

        // PUT: api/StockValues/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStockValues(int id, StockValues stockValues)
        {
            if (id != stockValues.Id)
            {
                return BadRequest();
            }

            _context.Entry(stockValues).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockValuesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StockValues
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StockValues>> PostStockValues(StockValues stockValues)
        {
          if (_context.StockValues == null)
          {
              return Problem("Entity set 'StocksContext.StockValues'  is null.");
          }
            _context.StockValues.Add(stockValues);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStockValues", new { id = stockValues.Id }, stockValues);
        }

        // DELETE: api/StockValues/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockValues(int id)
        {
            if (_context.StockValues == null)
            {
                return NotFound();
            }
            var stockValues = await _context.StockValues.FindAsync(id);
            if (stockValues == null)
            {
                return NotFound();
            }

            _context.StockValues.Remove(stockValues);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StockValuesExists(int id)
        {
            return (_context.StockValues?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
