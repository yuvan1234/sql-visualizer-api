using System;
using System.Web.Http;
using System.Web.Http.Cors;
using SQLVisualizer.Services;
using SQLViz.API.Models;

namespace SQLVisualizer.Controllers
{
    //[EnableCors(origins: "http://localhost:5173", headers: "*", methods: "*")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SqlVisualizerController : ApiController
    {
        private readonly SqlParser _sqlParser;

        public SqlVisualizerController()
        {
            _sqlParser = new SqlParser();
        }

        [HttpPost]
        [Route("api/visualize")]
        public IHttpActionResult VisualizeQuery([FromBody] QueryModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.SqlQuery))
                    return BadRequest("SQL query is required");

                var result = _sqlParser.ParseQuery(model.SqlQuery);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}