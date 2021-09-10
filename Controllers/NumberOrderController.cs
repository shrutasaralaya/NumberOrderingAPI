using Microsoft.AspNetCore.Mvc;
using NumberOrderingAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NumberOrderingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumberOrderController : ControllerBase
    {
        private readonly INumberOrderService _numberOrderService;
        public NumberOrderController(INumberOrderService numberOrderService)
        {
            _numberOrderService = numberOrderService;
        }

        [HttpPost]
        public async Task<IActionResult> StoreNumbersInOrder(List<int> unorderedNumbers)
        {
            if (unorderedNumbers == null || unorderedNumbers.Count <= 0)
            {
                return BadRequest("Please provide valid input");
            }

            var orderedList = _numberOrderService.SortUnOrderedList(unorderedNumbers);
            var isSuccuess =await _numberOrderService.StoreListIntoFile(orderedList);
            if (isSuccuess)
            {
                return Ok("Ordered Data saved Successfully ");
            }
            else
            {
                return StatusCode(500, "Unable to save data into file");
            }
            
        }

        [HttpGet]

        public IActionResult GetLatestFileData()
        {
            var latestOrderedList = _numberOrderService.GetLatestFileData();
            return Ok(latestOrderedList);
        }
    }
}
