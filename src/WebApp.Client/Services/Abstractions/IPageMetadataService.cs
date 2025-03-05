using Ingweland.Fog.Application.Client.Web.Models;

namespace Ingweland.Fog.WebApp.Client.Services.Abstractions;

public interface IPageMetadataService
{
    PageMetadata GetForCurrentPage();
}