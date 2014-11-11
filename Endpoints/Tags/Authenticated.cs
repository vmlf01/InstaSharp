using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InstaSharp.Model.Responses;

namespace InstaSharp.Endpoints.Tags {
    public class Authenticated : InstagramAPI {

        readonly Unauthenticated _unauthenticated;

        public Authenticated(InstagramConfig config, AuthInfo auth)
            : base(config, auth, "/tags/") {
                _unauthenticated = new Unauthenticated(config);
        }

        public TagResponse Get(string tagName) {
            return _unauthenticated.Get(tagName);
        }

        public string GetJson(string tagName) {
            return _unauthenticated.GetJson(tagName);
        }

        public MediasResponse Recent(string tagName) {
            return Recent(tagName, 15, null, null);
        }

        public MediasResponse Recent(string tagName, string min_id, string max_id) {
            return Recent(tagName, 15, min_id, max_id);
        }

        public MediasResponse Recent(string tagName, int count, string min_id, string max_id) {
            return (MediasResponse)Mapper.Map<MediasResponse>(RecentJson(tagName, min_id, max_id, count));
        }

        public TagsResponse Search(string searchTerm) {
            return _unauthenticated.Search(searchTerm);
        }

        private string RecentJson(string tagName, string min_id, string max_id, int count)
        {
            string uri = string.Format(base.Uri + "{0}/media/recent?client_id={1}&count={2}", tagName, InstagramConfig.ClientId, count);
            if (!string.IsNullOrEmpty(min_id)) uri += "&min_tag_id=" + min_id;
            if (!string.IsNullOrEmpty(max_id)) uri += "&max_tag_id=" + max_id;
            if (this.AuthInfo != null) uri += "&access_token=" + this.AuthInfo.Access_Token;

            return HttpClient.GET(uri);
        }
    }
}
