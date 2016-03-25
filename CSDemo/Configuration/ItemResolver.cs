using System.Linq;
using Sitecore;
using Sitecore.Data.ItemResolvers;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.IO;
using Sitecore.Links;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.SecurityModel;
using Sitecore.Sites;

namespace CSDemo.Configuration
{
    /// <summary>
    /// Resolves the current item from the query string path.
    /// </summary>
    public class ItemResolver : Sitecore.Pipelines.HttpRequest.ItemResolver
    {
        /// <summary>
        /// Runs the processor.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public override void Process(HttpRequestArgs args)
        {
            if (Context.Item != null || Context.Database == null || args.Url.ItemPath.Length == 0)
            {
                return;
            }
            // Try using classic
            var item = ResolveFullPath(args);
            if (item != null)
            {
                Context.Item = item;
                return;
            }

            // Fallback to default
            base.Process(args);
        }

        /// <summary>
        /// Resolves the full path.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        private Item ResolveFullPath(HttpRequestArgs args)
        {
            string itemPath = args.Url.ItemPath;
            if (string.IsNullOrEmpty(itemPath) || itemPath[0] != '/')
            {
                return null;
            }
            int num = itemPath.IndexOf('/', 1);
            if (num < 0)
            {
                return null;
            }
            Item item = ItemManager.GetItem(itemPath.Substring(0, num), Language.Current, Sitecore.Data.Version.Latest, Context.Database, SecurityCheck.Disable);
            if (item == null)
            {
                return null;
            }
            string path = itemPath.Substring(num);
            Item item2 = this.PathResolver.ResolveItem(path, item);
            if (item2 == null && args.LocalPath.Length > num)
            {
                path = args.LocalPath.Substring(num);
                item2 = this.PathResolver.ResolveItem(path, item);
            }
            return item2;
        }
    }
}
