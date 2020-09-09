namespace Rtl.Assignment.Api.Abstractions.Response
{
    using System;

    public class PersonResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Birthday { get; set; }
    }
}