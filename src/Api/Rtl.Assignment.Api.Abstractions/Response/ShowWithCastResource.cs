namespace Rtl.Assignment.Api.Abstractions.Response
{
    using System.Collections.Generic;

    public class ShowWithCastResource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public IEnumerable<PersonResource> Cast { get; set; }
    }
}