using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.OData;
using System.Web.OData.Extensions;
using System.Web.OData.Query;
using Skarpline.Common.Identity;
using Skarpline.Common.Results;
using Skarpline.Services;

namespace Skarpline.Api.Controllers
{
    /// <summary>
    /// Base API controller
    /// </summary>
    [Authorize]
    public abstract class BaseApiController : ApiController
    {
        /// <summary>
        /// Prepares error result if any
        /// </summary>
        /// <param name="result">Response from BLL layer to parse</param>
        /// <returns></returns>
        protected IHttpActionResult GetErrorResult(ServiceResult result)
        {
            if (result == null) return InternalServerError();
            if (result.Succeeded) return null;

            if (result.Errors != null)
            {
                int count = 0;
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError($"Error_{++count}", error);
                }
            }

            if (ModelState.IsValid) return BadRequest();
            return BadRequest(ModelState);
        }
    }

    /// <summary>
    /// Generic Base API controller
    /// </summary>
    /// <typeparam name="T">Type of entity to which controller belongs</typeparam>
    /// <typeparam name="TModel">View model of controller entity</typeparam>
    /// <typeparam name="TIdentifier">Id which is used in Entity and it's ViewModel</typeparam>
    [Authorize]
    public abstract class BaseApiController<T, TModel, TIdentifier> : BaseApiController
        where T : class, IIdentifier<TIdentifier> where TIdentifier : struct
        where TModel : class, IIdentifier<TIdentifier>
    {
        protected readonly IService<T> EntityService;
        protected string[] Includes { get; set; }

        protected BaseApiController(IService<T> entityService)
        {
            EntityService = entityService;
        }

        /// <summary>
        /// GET api/<controller>
        /// </summary>
        /// <param name="query">OData query</param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> Get(ODataQueryOptions<T> query)
        {
            var entities = await query.ApplyTo(EntityService.GetAll(Includes)).ToListAsync();
            var result = AutoMapper.Mapper.Map<IEnumerable<TModel>>(entities);

            var pageResult = new PageResult<TModel>(result,
                Request.ODataProperties().NextLink,
                Request.ODataProperties().TotalCount);

            return Ok(pageResult);
        }

        /// <summary>
        /// GET api/<controller>/5
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        public virtual IHttpActionResult Get(int id)
        {
            var entity = EntityService.GetByKey(id);
            if (entity == null) return NotFound();
            var result = AutoMapper.Mapper.Map<TModel>(entity);
            return Ok(result);
        }

        /// <summary>
        /// POST api/<controller>
        /// </summary>
        /// <param name="value">Entity to Create</param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> Post([FromBody]TModel value)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var entity = AutoMapper.Mapper.Map<T>(value);
            var result = await EntityService.CreateAsync(entity);
            GetErrorResult(result);
            value = AutoMapper.Mapper.Map<TModel>(entity);
            return Created("DefaultApi", value);
        }

        /// <summary>
        /// PUT api/<controller>
        /// </summary>
        /// <param name="id">Entity Id</param>
        /// <param name="value">Entity to update</param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> Put(int id, [FromBody]TModel value)
        {
            var result = EntityService.GetByKey(id);
            if (result == null) return NotFound();
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var entity = AutoMapper.Mapper.Map(value, result);
            var updateResult = await EntityService.UpdateAsync(entity);
            GetErrorResult(updateResult);
            var mapperValue = AutoMapper.Mapper.Map<TModel>(entity);
            return Ok(mapperValue);
        }

        /// <summary>
        /// DELETE api/<controller>/5
        /// </summary>
        /// <param name="id">Entity ID</param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> Delete(TIdentifier id)
        {
            var result = EntityService.GetByKey(id);
            if (result == null) return NotFound();
            var deleteResult = await EntityService.DeleteAsync(result);
            GetErrorResult(deleteResult);
            return Ok();
        }

        /// <summary>
        /// Deletes entity by it's ViewModel
        /// </summary>
        /// <param name="value">Entity to delete</param>
        /// <returns></returns>
        public virtual async Task<IHttpActionResult> Delete([FromBody]TModel value)
        {
            return await Delete(value.Id);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) EntityService.Dispose();
            base.Dispose(disposing);
        }
    }
}
