using Ardalis.ApiEndpoints;

using BlazingTrails.Api.Persistence;
using BlazingTrails.Shared.Features.ManageTrails.EditTrail;
using BlazingTrails.Shared.Features.ManageTrails.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlazingTrails.Api.Features.ManageTrails.EditTrail
{
    public class EditTrailEndpoint : EndpointBaseAsync.WithRequest<EditTrailRequest>.WithActionResult<EditTrailRequest.Response>
    {
        private readonly BlazingTrailsContext _context;

        public EditTrailEndpoint(BlazingTrailsContext context)
        {
            _context = context;
        }

        [HttpPut(EditTrailRequest.Route)]
        public override async Task<ActionResult<EditTrailRequest.Response>> HandleAsync(EditTrailRequest request, CancellationToken cancellationToken = default)
        {
            var trail = await _context.Trails.Include(x => x.Route).SingleOrDefaultAsync(s => s.Id == request.Trail.Id, cancellationToken);

            if (trail is null)
                BadRequest("The trail could not be found.");

            trail.Name = request.Trail.Name;
            trail.Description = request.Trail.Description;
            trail.Location = request.Trail.Location;
            trail.TimeInMinutes = request.Trail.TimeInMinutes;
            trail.Length = request.Trail.Length;
            trail.Route = request.Trail.Route.Select(s => new Persistence.Entities.RouteInstruction { Trail = trail, Stage = s.Stage, Description = s.Description }).ToList();

            if(request.Trail.ImageAction == ImageAction.Remove)
            {
                System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "images", request.Trail.Image!));
                trail.Image = null;
            }

            await _context.SaveChangesAsync();

            return Ok(true);
        }
    }
}
