﻿using BlazingTrails.Shared.Features.ManageTrails.EditTrail;

using MediatR;

using System.Net.Http.Json;

namespace BlazingTrails.Client.Features.ManageTrails.EditTrail
{
    public class GetTrailHandler : IRequestHandler<GetTrailRequest, GetTrailRequest.Response>
    {
        private readonly HttpClient _httpClient;
        public GetTrailHandler(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetTrailRequest.Response> Handle(GetTrailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<GetTrailRequest.Response>(GetTrailRequest.Route.Replace("{trailId}", request.TrailId.ToString()), cancellationToken);
            }
            catch (HttpRequestException)
            {
                return default!;
            }
        }
    }
}