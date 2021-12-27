using FlightInfo.BlazorApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInfo.BlazorApp.Shared.Parsing
{
    public interface IParser
    {
        public NotamPart Parse(Notam notam, string value);
    }
}
