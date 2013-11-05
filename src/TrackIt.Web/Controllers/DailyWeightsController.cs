using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TrackIt.Web.Data;

namespace TrackIt.Web.Controllers
{
    public class DailyWeightsController : ApiController 
    {
        private readonly IDailyWeightRepository _repository;

        public DailyWeightsController(IDailyWeightRepository repository)
        {
            _repository = repository;
        }


        public IEnumerable<DailyWeight> Get()
        {
            if (User.Identity.IsAuthenticated)
            {
                IQueryable<DailyWeight> results = _repository.GetWeightsByUser(User.Identity.Name);
                return results.OrderByDescending(t => t.WeightDate).Take(50).ToList();
            }
            return new DailyWeight[0];
        }

        public HttpResponseMessage Post([FromBody] DailyWeight newWeight)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (newWeight.WeightDate == default(DateTime))
                {
                    newWeight.WeightDate = DateTime.Today;
                }
                newWeight.UserName = User.Identity.Name;
                if (_repository.AddDailyWeight(newWeight) && _repository.Save())
                {
                    return Request.CreateResponse(HttpStatusCode.Created, newWeight);
                }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return Request.CreateResponse(HttpStatusCode.Unauthorized);
        }
    }
}