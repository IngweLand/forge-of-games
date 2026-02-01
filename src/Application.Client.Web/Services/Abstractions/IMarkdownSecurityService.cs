namespace Ingweland.Fog.Application.Client.Web.Services.Abstractions;

public interface IMarkdownSecurityService
{
    string ConvertToSafeHtml(string markdown);
}
