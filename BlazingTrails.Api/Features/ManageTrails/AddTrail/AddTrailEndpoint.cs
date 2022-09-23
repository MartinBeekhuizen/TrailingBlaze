using Ardalis.ApiEndpoints;
using BlazingTrails.Api.Persistence;
using Microsoft.AspNetCore.Mvc;
using BlazingTrails.Api.Persistence.Entities;
using BlazingTrails.Shared.Features.ManageTrails.AddTrail;

namespace BlazingTrails.Api.Features.ManageTrails.AddTrail
{
    public class AddTrailEndpoint : EndpointBaseAsync.WithRequest<AddTrailRequest>.WithActionResult<int>
    {
        private readonly BlazingTrailsContext _database;

        public AddTrailEndpoint(BlazingTrailsContext database)
        {
            _database = database;
        }

        [HttpPost(AddTrailRequest.RouteTemplate)]
        public override async Task<ActionResult<int>> HandleAsync(AddTrailRequest request, CancellationToken cancellationToken = default)
        {
            var trail = new Trail
            {
                Name = request.Trail.Name,
                Description = request.Trail.Description,
                Location = request.Trail.Location,
                TimeInMinutes = request.Trail.TimeInMinutes,
                Length = request.Trail.Length

            };

            await _database.Trails.AddAsync(trail, cancellationToken);

            var routeInstructions = request.Trail.Route.Select(s => new RouteInstruction { Stage = s.Stage, Description = s.Description, Trail = trail });

            await _database.RouteInstructions.AddRangeAsync(routeInstructions);

            await _database.SaveChangesAsync();

            return Ok(trail.Id);
        }
    }
}
