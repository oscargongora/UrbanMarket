using ChicStreetwear.Application.Common.Commands;

namespace ChicStreetwear.Api;

public static partial class Requests
{
    public sealed class PictureRequest
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
        public PictureCommand ToCommand() => new(Name, Url);
    };

    public sealed class GetPicturesRequest : GetRequestBase {
        public Guid? userId { get; set; }
    };
}
