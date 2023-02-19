using Snitch.Core.Abstractions;
using Snitch.Core.Models;
using System;
using System.Threading.Tasks;

namespace Snitch.Core.Web.Response
{
    [Serializable]
    internal class GetMaessageRequest : IRequestData
    {
       public Message Message { get; init; }
    }
}