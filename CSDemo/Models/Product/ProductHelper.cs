#region

using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.LuceneProvider;
using Sitecore.ContentSearch.Linq;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using CSDemo;
using Sitecore.Data;
using System;
using Sitecore.Diagnostics;
using Glass.Mapper.Sc;
using Sitecore.Links;

#endregion

namespace CSDemo.Models.Product
{
    public static class ProductHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static string GetFirstImageFromProductItem(Item product)
        {
            var ProductImageIds = product[Constants.Products.ImagesField];
            var ProductImage = string.Empty;
            if (!string.IsNullOrEmpty(ProductImageIds))
            {
                ProductImage = GetFirstImageFromField(ProductImageIds);
            }

            return ProductImage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imageIds"></param>
        /// <returns></returns>
        public static string GetFirstImageFromField(string imageIds)
        {
            var image = string.Empty;

            if (!string.IsNullOrEmpty(imageIds))
            {
                var images = imageIds.Split(Constants.Products.IDSeparator.ToCharArray());

                if (!string.IsNullOrEmpty(images[0]))
                {
                    image = string.Format(Constants.Products.ImagesUrlFormat, ID.Parse(images[0]).ToShortID());
                }

                
            }
            return image;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="images"></param>
        /// <returns></returns>
        public static string GetFirstImageFromItems(IEnumerable<Item> images)
        {
            var image = string.Empty;

            if (images != null)
            {
                if (images.Count() > 0)
                {
                    var firstImage = images.First();
                    image = string.Format(Constants.Products.ImagesUrlFormat, firstImage.ID.ToShortID());
                }

            }
            return image;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoryIDs"></param>
        /// <returns></returns>
        public static List<Category> GetCategories(string categoryIDs)
        {

            var categories = new List<Category>();

            if (!string.IsNullOrEmpty(categoryIDs))
            {
                try
                {

                    foreach (var categoryID in categoryIDs.Split(Constants.Products.IDSeparator.ToCharArray()))
                    {
                        var category = new Category();

                        SearchResultItem searchedItem = GetSearchResultItemById(categoryID);

                        category = searchedItem.GetItem().GlassCast<Category>();

                        categories.Add(category);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex.StackTrace, ex);

                }                          
            }

            return categories;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns></returns>
        public static SearchResultItem GetSearchResultItemById(string itemID)
        {
            var index = ContentSearchManager.GetIndex(Constants.WebIndex);
            try
            {
                var culture = Sitecore.Context.Language.CultureInfo;
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Sitecore.Context.Language.Name);
                    return queryable.FirstOrDefault(x => x.ItemId == ID.Parse(itemID));

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, ex);

            }

            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal static CategoryProductViewModel GetCategoryProducts(PaginationViewModel model)
        {
            var categoryProductVM = new CategoryProductViewModel();

            categoryProductVM.PaginationViewModel = model;

            var category = new Category();

            categoryProductVM.CategoryMenulist = GetCategoryMenuList(Constants.Products.CategoriesParentId);

            SearchResultItem searchedItem = GetSearchResultItemById(model.CategoryID);

            if (searchedItem != null)
            {           
                var catItem = searchedItem.GetItem();
                
                category = catItem.GlassCast<Category>();

                if (catItem.HasChildren)
                {
                    
                    var catChildren = catItem.GetChildren().Select(x => x.GlassCast<Product>()).ToList();

                    model.TotalItems = catChildren.Count();

                    model.TotalPages = (long)Math.Ceiling((double)model.TotalItems / model.PageSize);

                    // do sorting
                    if (!string.IsNullOrEmpty(model.OrderBy))
                    {
                        switch (model.OrderBy)
                        {
                            case Constants.Products.OrderByBrand:
                                catChildren = catChildren.OrderBy(x => x.Brand).ToList();
                                break;

                            case Constants.Products.OrderByNewness:
                                catChildren = catChildren.OrderByDescending(x => x.DateOfIntroduction).ToList();
                                break;

                            case Constants.Products.OrderByRatings:
                                catChildren = catChildren.OrderByDescending(x => x.Rating).ToList();
                                break;

                            case Constants.Products.OrderByPriceAsc:
                                catChildren = catChildren.OrderBy(x => x.ListPrice).ToList();
                                break;

                            case Constants.Products.OrderByPriceDesc:
                                catChildren = catChildren.OrderByDescending(x => x.ListPrice).ToList();
                                break;

                            default:
                                break;

                        }

                    }
                
                    // do paging
                    category.Products = catChildren
                        .Skip(model.PageSize * (model.CurrentPage - 1))
                        .Take(model.PageSize);

                    // Update ProductVariants
                    foreach (var Product in category.Products)
                    {
                        Item catProdItem = catItem.GetChildren().FirstOrDefault(x => x.ID.ToGuid() == Product.ID);
                        if (catProdItem != null && catProdItem.HasChildren)
                        {
                            Product.ProductVariants = catProdItem.GetChildren().Select(x=>x.GlassCast<ProductVariant>());                         
                        }
                    }  
                }

            }

            categoryProductVM.Category = category;

            return categoryProductVM;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private static IEnumerable<CategoryMenulistViewModel> GetCategoryMenuList(string parentID)
        {
            var CategoryMenulistViewModel = new List<CategoryMenulistViewModel>();
            
            List<SearchResultItem> categories = GetCategoryMenuListByParentID(parentID);

            if (categories != null)
            {

                foreach (SearchResultItem category in categories)
                {
                    var c = new CategoryMenulistViewModel();
                    c.ID = category.ItemId.ToString();
                    c.Name = category.Name;
                    var categoryChildern = category.GetItem().GetChildren();
                    c.ProductsCount = categoryChildern.Count();

                    var pList = new List<ProductMenulistViewModel>();

                    foreach (Item categoryChild in categoryChildern)
                    {
                        var p = new ProductMenulistViewModel();
                        p.Name = categoryChild.DisplayName;
                        p.Url = LinkManager.GetItemUrl(categoryChild);
                        pList.Add(p);
                    }
                    c.ProductMenulistViewModel = pList;
                    CategoryMenulistViewModel.Add(c);
                    
                }

            }
            CategoryMenulistViewModel = CategoryMenulistViewModel.OrderBy(i => i.Name).ToList();
            return CategoryMenulistViewModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentID"></param>
        /// <returns></returns>
        private static List<SearchResultItem> GetCategoryMenuListByParentID(string parentID)
        {
            var index = ContentSearchManager.GetIndex(Constants.WebIndex);
            try
            {
                var culture = Sitecore.Context.Language.CultureInfo;
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Sitecore.Context.Language.Name);
                    return queryable.Where(x => x.Parent == ID.Parse(parentID)).ToList();

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, ex);

            }

            return null;

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemName"></param>
        /// <param name="parentID"></param>
        /// <returns></returns>
        internal static string GetItemIDFromName(string itemName, string parentID)
        {
            var index = ContentSearchManager.GetIndex(Constants.WebIndex);
            try
            {
                var culture = Sitecore.Context.Language.CultureInfo;
                using (var context = index.CreateSearchContext())
                {
                    var queryable = context.GetQueryable<SearchResultItem>()
                        .Where(x => x.Language == Sitecore.Context.Language.Name);
                    return queryable.FirstOrDefault(x => String.Equals(x.Name, itemName, StringComparison.CurrentCultureIgnoreCase) && x.Parent == ID.Parse(parentID)).ItemId.ToString();

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace, ex);

            }

            return string.Empty;

        }
    }
}