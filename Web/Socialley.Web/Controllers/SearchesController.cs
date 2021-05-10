namespace Socialley.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Socialley.Services.Data;

    [ApiController]
    [Route("api/[controller]")]
    public class SearchesController : BaseController
    {
        private readonly ISearchesService searchesService;

        public SearchesController(ISearchesService searchesService)
        {
            this.searchesService = searchesService;
        }

        public ActionResult<string[]> Searches()
        {
            var responseModel = this.searchesService.Searches();

            return responseModel;
        }
    }
}
