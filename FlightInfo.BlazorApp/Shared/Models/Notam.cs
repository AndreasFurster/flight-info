using FlightInfo.BlazorApp.Shared.Parsing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightInfo.BlazorApp.Shared.Models
{
    public class Notam
    {
        [Required]
        public string? NotamInput { get; set; }

        public bool MakeReadable { get; set; } = false;

        public IEnumerable<NotamPart>? NotamParts { get; set; }

        public bool IsParsed { get; set; } = false;

        public void ParseInput()
        {
            if(NotamInput == null)
            {
                throw new ArgumentNullException("NotamInput is null");
            }

            var notamParts = new List<NotamPart>();

            var lines = NotamInput.Split('\n');
            
            var identificationParser = new IdentificationParser();
            notamParts.Add(identificationParser.Parse(this, lines[0]));

            NotamParts = notamParts;

            IsParsed = true;
        }
    }
}
