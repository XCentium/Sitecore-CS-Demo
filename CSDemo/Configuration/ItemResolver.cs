using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Globalization;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.SecurityModel;

namespace CSDemo.Configuration
{
    /// <summary>
    ///     Resolves the current item from the query string path.
    /// </summary>
    public class ItemResolver : Sitecore.Pipelines.HttpRequest.ItemResolver
    {
        /// <summary>
        ///     Runs the processor.
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
        ///     Resolves the full path.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        private Item ResolveFullPath(HttpRequestArgs args)
        {
            var itemPath = args.Url.ItemPath;
            if (string.IsNullOrEmpty(itemPath) || itemPath[0] != '/')
            {
                return null;
            }
            var num = itemPath.IndexOf('/', 1);
            if (num < 0)
            {
                return null;
            }
            var item = ItemManager.GetItem(itemPath.Substring(0, num), Language.Current, Version.Latest,
                Context.Database, SecurityCheck.Disable);
            if (item == null)
            {
                return null;
            }
            var path = itemPath.Substring(num);
            var item2 = PathResolver.ResolveItem(path, item);
            if (item2 == null && args.LocalPath.Length > num)
            {
                path = args.LocalPath.Substring(num);
                item2 = PathResolver.ResolveItem(path, item);
            }
            return item2;
        }
    }
}