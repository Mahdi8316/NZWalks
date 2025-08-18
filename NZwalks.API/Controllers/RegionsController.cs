using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZwalks.API.Data;
using NZwalks.API.Models.Domain;
using NZwalks.API.Models.DTO;
using NZwalks.API.Repositories;

namespace NZwalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZWalksDbContext dbContext,IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regions = await regionRepository.GetAllAsync();
            //var regions =await  dbContext.Regions.ToListAsync();
            var regionsDto = new List<RegionDTO>();
            foreach (var region in regions)
            {
                regionsDto.Add(new RegionDTO()
                {
                    id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }


            return Ok(regionsDto);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {

            //var region = dbContext.Regions.Find(id); just for primary key
            var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
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
        public async Task<IActionResult> Create([FromBody] AddRequestRegionDto addRequestRegionDto)
        {
            var region = new Region
            {
                Code = addRequestRegionDto.Code,
                Name = addRequestRegionDto.Name,
                RegionImageUrl= addRequestRegionDto.RegionImageUrl
            };
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById),new {id = region.Id},region);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRequestRegionDto updateRequestRegionDto)
        {
            var regionDTO = await dbContext.Regions.FirstOrDefaultAsync(x =>x.Id == id);
            if(regionDTO == null)
            {
                return NotFound();
            }
            regionDTO.Name = updateRequestRegionDto.Name;
            regionDTO.Code = updateRequestRegionDto.Code;
            regionDTO.RegionImageUrl = updateRequestRegionDto.RegionImageUrl;
            await dbContext.SaveChangesAsync();
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
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var x = await dbContext.Regions.FirstOrDefaultAsync(y => y.Id == id);
            if(x == null) {  return NotFound(); }
            dbContext.Regions.Remove(x);
            await dbContext.SaveChangesAsync();
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
