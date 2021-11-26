using System;
using System.Net.Http.Headers;
using System.Text;

namespace Bomberjam.Website.Authentication
{
    public sealed class BasicAuthenticationHeaderValue : AuthenticationHeaderValue
    {
        public BasicAuthenticationHeaderValue(string username, string password)
            : base("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ':' + password)))
        {
        }
    }
}