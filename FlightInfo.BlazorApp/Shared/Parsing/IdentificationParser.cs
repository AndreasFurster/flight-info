using FlightInfo.BlazorApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FlightInfo.BlazorApp.Shared.Parsing
{
    public class IdentificationParser : IParser
    {
        public NotamPart Parse(Notam notam, string value)
        {
            var identificationPart = new NotamPart(notam, value);
            identificationPart.ReadableValue = "Identification: ";

            identificationPart.NotamParts = ParseLine(identificationPart, value);

            return identificationPart;
        }

        private IEnumerable<NotamPart>? ParseLine(NotamPart parent, string value)
        {
            // Eg: A1234/06 NOTAMR A1212/0
            string pattern = @"(?<id>.*)(?<type>NOTAM[NRC])(?<replacing_id>.*)";
            RegexOptions options = RegexOptions.IgnorePatternWhitespace;

            var matches = Regex.Matches(value, pattern, options);
            if (matches.Count == 0) return null;

            var match = matches[0];

            var parts = new List<NotamPart>();

            if(match.Groups.ContainsKey("id"))
            { 
                parts.Add(ParseId(parent, match.Groups["id"].Value));
            }

            if (match.Groups.ContainsKey("type"))
            {
                parts.Add(ParseType(parent, match.Groups["type"].Value));
            }

            if (match.Groups.ContainsKey("replacing_id"))
            {
                parts.Add(ParseId(parent, match.Groups["replacing_id"].Value));
            }

            return parts.Any() ? parts : null;
        }

        private NotamPart ParseId(NotamPart parent, string value)
        {
            var part = new NotamPart(parent, value.Trim());

            var pattern = @"(?<id>[A-Z]{1}[0-9]{4})\/(?<year>[0-9]{2})";
            RegexOptions options = RegexOptions.IgnorePatternWhitespace;

            var matches = Regex.Matches(value, pattern, options);
            if (matches.Count == 0) return part;

            var match = matches[0];

            if (match.Groups.ContainsKey("id") && match.Groups.ContainsKey("year"))
            { 
                var id = new NotamPart(parent, match.Groups["id"].Value);
                var year = new NotamPart(parent, match.Groups["year"].Value);

                id.ReadableValue = $"notam {id.Value}";

                // BUG: 1900 does not exist. Also think about when to switch from 19 to 20. 
                year.ReadableValue = $"from 20{year.Value}";

                part.NotamParts = new List<NotamPart> { id, year };
            }

            return part;
        }

        private NotamPart ParseType(NotamPart parent, string value)
        {
            var part = new NotamPart(parent, value.Trim());
            part.ReadableValue = part.Value switch
            {
                "NOTAMN" => "is added",
                "NOTAMR" => "is replacing",
                "NOTAMC" => "is cancelled",
                _ => part.Value
            };

            return part;
        }

    }
}
