using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeSecurityAPI.DataAccess;
using HomeSecurityAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeSecurityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        public DataAccessPictures dap = new DataAccessPictures();

        // GET api/service/date
        [HttpGet]
        public async Task<List<Picture>> GetByDate(string stringDate)
        {
            //Works with this format "YYYY-MM-DD, h:mm:ss a"
            DateTime date = DateTime.Parse(stringDate, null);
            return await dap.GetPicturesByDate(date);
        }

        //POST api/service
        [HttpPost]
        public async Task<List<Picture>> RequestByDate(string stringDate1, string stringDate2)
        {
            //Works with this format "YYYY-MM-DD, h:mm:ss a"
            DateTime date1 = Convert.ToDateTime(stringDate1);
            DateTime date2 = Convert.ToDateTime(stringDate2);
            return await dap.GetPictureByInterval(date1, date2);
        }
    }
}