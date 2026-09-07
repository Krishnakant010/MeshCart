using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;

namespace Discount.Application.Extensions;

public static class GrpcErrorHelper
{
    public static RpcException CreateValidationException(Dictionary<string, string> fieldErrors)
    {
        var fieldVoilations = new List<BadRequest.Types.FieldViolation>();
        foreach (var errpr in fieldErrors)
        {
            
            fieldVoilations.Add( new BadRequest.Types.FieldViolation
            {
                Field = errpr.Key,
                Description = errpr.Value
            });
        }

        var badRequest = new BadRequest();
        badRequest.FieldViolations.AddRange(fieldVoilations);
        var status = new Google.Rpc.Status
        {
            Code = (int)StatusCode.InvalidArgument,
            Message = "Validation Failed",
            Details = { Any.Pack(badRequest)}
        };
        var trailers = new Metadata
        {
            { "grpc-status-details-bin", status.ToByteArray() }
        };
        
        return new RpcException(new global::Grpc.Core.Status(StatusCode.InvalidArgument, "Validation Errors"), trailers);
    }
}