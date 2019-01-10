using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HomeSecurityAPI.DataAccess;
using HomeSecurityAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace HomeSecurityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PictureController : ControllerBase
    {
        private DataAccessPictures dap = new DataAccessPictures();
        private readonly IHostingEnvironment hostingEnv;
        private const string pictruFolder = "Images";

        public PictureController(IHostingEnvironment hostingEnv)
        {
            if (hostingEnv != null)
                this.hostingEnv = hostingEnv;
            else
                throw new ArgumentNullException(nameof(hostingEnv));
        }

        // GET api/picture/id
        [HttpGet("id")]
        [Route("GetAll")]
        public async Task<List<Picture>> GetAll(int id)
        {
           return  await dap.GetAllbyUserID(id);
        }

        // GET api/picture/pictureName
        [HttpGet("pictureName")]
        [Route("Get")]
        public ActionResult Get(string pictureName)
        {
            var path = Path.Combine(hostingEnv.WebRootPath, pictruFolder, pictureName);
            return PhysicalFile(path, "image/jpeg");
        }

        // POST api/picture
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Picture p)
        {
            var response = await dap.Create(p);
            return Ok(response);
        }

        // DELETE api/picture/objId
        [HttpDelete]
        public async Task<IActionResult> Delete(string objId)
        {
            return Ok(await dap.Delete(objId));
        }
    }
}