using FlightInfo.BlazorApp.Shared.Parsing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInfo.BlazorApp.Shared.Models
{
    public class NotamPart
    {
        public Notam Notam { get; }

        public string Value { get; }

        public int Level { get; }

        public string? ReadableValue { get; set; }

        public string DisplayValue() => Notam.MakeReadable ? ReadableValue ?? Value : Value;

        public Color Color { get; set; } = Color.LightBlue;

        public IEnumerable<NotamPart>? NotamParts { get; set; }

        public NotamPart(Notam notam, string value)
        {
            Notam = notam;
            Value = value;
            Level = 1;
        }

        public NotamPart(NotamPart parent, string value)
        {
            Value = value.Trim();
            Level = parent.Level + 1;
            Notam = parent.Notam;
        }

        public void ParseInput()
        {
           
        }
    }
}
