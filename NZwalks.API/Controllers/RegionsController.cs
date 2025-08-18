using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZwalks.API.Data;
using NZwalks.API.Models.Domain;
using NZwalks.API.Models.DTO;

namespace NZwalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var regions = dbContext.Regions.ToList();
            var regionsDto = new List<RegionDTO>();
            foreach (var region in regions)
            {
                regionsDto.Add(new RegionDTO()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }


            return Ok(regionsDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {

            //var region = dbContext.Regions.Find(id); just for primary key
            var region = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if(region == null)
            {
                return NotFound();
            }
            var regionsDto = new RegionDTO
            {
                
                Code = region.Code,
                Name = region.Name,
                RegionImageUrl = region.RegionImageUrl
            };

            return Ok(regionsDto);
        }
        [HttpPost]
        public IActionResult Create([FromBody] AddRequestRegionDto addRequestRegionDto)
        {
            var region = new Region
            {
                Code = addRequestRegionDto.Code,
                Name = addRequestRegionDto.Name,
                RegionImageUrl= addRequestRegionDto.RegionImageUrl
            };
            dbContext.Regions.Add(region);
            dbContext.SaveChanges();

            return CreatedAtAction(nameof(GetById),new {id = region.Id},region);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRequestRegionDto updateRequestRegionDto)
        {
            var regionDTO = dbContext.Regions.FirstOrDefault(x =>x.Id == id);
            if(regionDTO == null)
            {
                return NotFound();
            }
            regionDTO.Name = updateRequestRegionDto.Name;
            regionDTO.Code = updateRequestRegionDto.Code;
            regionDTO.RegionImageUrl = updateRequestRegionDto.RegionImageUrl;
            dbContext.SaveChanges();
            var x = new RegionDTO
            {
                Name = regionDTO.Name,
                RegionImageUrl = regionDTO.RegionImageUrl,
                Code = regionDTO.Code,
                id = regionDTO.Id
            };
            return Ok(x);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var x = dbContext.Regions.FirstOrDefault(y => y.Id == id);
            if(x == null) {  return NotFound(); }
            dbContext.Regions.Remove(x);
            dbContext.SaveChanges();
            var z = new RegionDTO
            {
                Name = x.Name,
                RegionImageUrl = x.RegionImageUrl,
                Code = x.Code,
                id = x.Id
            };
            return Ok(z);
        }
    }
}
