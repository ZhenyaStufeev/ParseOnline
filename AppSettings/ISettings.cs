using AppSettings.Entitites;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppSettings
{
    public interface ISettings
    {
        string baseURL { get; }
        ProfileSettings profile { get; }
        TeamSettings team { get; }
    }
}
