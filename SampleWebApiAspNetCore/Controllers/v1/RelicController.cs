using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Services;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using System.Text.Json;

namespace SampleWebApiAspNetCore.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RelicController : ControllerBase
    {
        private readonly IRelicRepository _relicRepository;
        private readonly IMapper _mapper;
        private readonly ILinkService<RelicController> _linkService;

        public RelicController(
            IRelicRepository relicRepository,
            IMapper mapper,
            ILinkService<RelicController> linkService)
        {
            _relicRepository = relicRepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet(Name = nameof(GetALLRelic))]
        public ActionResult GetALLRelic(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<RelicEntity> relicItems = _relicRepository.GetAll(queryParameters).ToList();

            var allItemCount = _relicRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = _linkService.CreateLinksForCollection(queryParameters, allItemCount, version);
            var toReturn = relicItems.Select(x => _linkService.ExpandSingleRelicItem(x, x.Id, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetOneRelic))]
        public ActionResult GetOneRelic(ApiVersion version, int id)
        {
            RelicEntity relicItem = _relicRepository.GetSingle(id);

            if (relicItem == null)
            {
                return NotFound();
            }

            RelicDto item = _mapper.Map<RelicDto>(relicItem);

            return Ok(_linkService.ExpandSingleRelicItem(item, item.Id, version));
        }

        [HttpPost(Name = nameof(AddRelic))]
        public ActionResult<RelicDto> AddRelic(ApiVersion version, [FromBody] RelicCreateDto relicCreateDto)
        {
            if (relicCreateDto == null)
            {
                return BadRequest();
            }

            RelicEntity toAdd = _mapper.Map<RelicEntity>(relicCreateDto);

            _relicRepository.Add(toAdd);

            if (!_relicRepository.Save())
            {
                throw new Exception("Reolling a relic item failed on save.");
            }

            RelicEntity newRelicItem = _relicRepository.GetSingle(toAdd.Id);
            RelicDto RelicDto = _mapper.Map<RelicDto>(newRelicItem);

            return CreatedAtRoute(nameof(GetOneRelic),
                new { version = version.ToString(), id = newRelicItem.Id },
                _linkService.ExpandSingleRelicItem(RelicDto, RelicDto.Id, version));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateRelic))]
        public ActionResult<RelicDto> PartiallyUpdateRelic(ApiVersion version, int id, [FromBody] JsonPatchDocument<RelicUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            RelicEntity existingEntity = _relicRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            RelicUpdateDto RelicUpdateDto = _mapper.Map<RelicUpdateDto>(existingEntity);
            patchDoc.ApplyTo(RelicUpdateDto);

            TryValidateModel(RelicUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(RelicUpdateDto, existingEntity);
            RelicEntity updated = _relicRepository.Update(id, existingEntity);

            if (!_relicRepository.Save())
            {
                throw new Exception("Updating a fooditem failed on save.");
            }

            RelicDto RelicDto = _mapper.Map<RelicDto>(updated);

            return Ok(_linkService.ExpandSingleRelicItem(RelicDto, RelicDto.Id, version));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveRelic))]
        public ActionResult RemoveRelic(int id)
        {
            RelicEntity relicItem = _relicRepository.GetSingle(id);

            if (relicItem == null)
            {
                return NotFound();
            }

            _relicRepository.Delete(id);

            if (!_relicRepository.Save())
            {
                throw new Exception("Deleting a relic item failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateRelic))]
        public ActionResult<RelicDto> UpdateRelic(ApiVersion version, int id, [FromBody] RelicUpdateDto RelicUpdateDto)
        {
            if (RelicUpdateDto == null)
            {
                return BadRequest();
            }

            var existingRelicItem = _relicRepository.GetSingle(id);

            if (existingRelicItem == null)
            {
                return NotFound();
            }

            _mapper.Map(RelicUpdateDto, existingRelicItem);

            _relicRepository.Update(id, existingRelicItem);

            if (!_relicRepository.Save())
            {
                throw new Exception("Updating a relic item failed on save.");
            }

            RelicDto RelicDto = _mapper.Map<RelicDto>(existingRelicItem);

            return Ok(_linkService.ExpandSingleRelicItem(RelicDto, RelicDto.Id, version));
        }

        [HttpGet("GetRandomRelic", Name = nameof(GetRandomRelic))]
        public ActionResult GetRandomRelic()
        {
            ICollection<RelicEntity> relicItems = _relicRepository.GetRandomRelic();

            IEnumerable<RelicDto> dtos = relicItems.Select(x => _mapper.Map<RelicDto>(x));

            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(Url.Link(nameof(GetRandomRelic), null), "self", "GET"));

            return Ok(new
            {
                value = dtos,
                links = links
            });
        }
    }
}
