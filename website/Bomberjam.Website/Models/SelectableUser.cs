using System;

namespace Bomberjam.Website.Models
{
    public class SelectableUser
    {
        public Guid Id { get; set; }
        public bool IsSelected { get; set; }
    }
}