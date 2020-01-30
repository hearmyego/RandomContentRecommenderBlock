using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using RandomContentRecommendation.Models.Pages;
using RandomContentRecommendation.Models.ViewModels;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Filters;
using EPiServer.Web.Mvc;

namespace RandomContentRecommendation.Controllers
{
    public class RandomContentRecommenderBlockController : BlockController<Models.Blocks.RandomContentRecommenderBlock>
    {
        private readonly IPageCriteriaQueryService _pageCriteriaQueryService;
        private readonly IContentTypeRepository _contentTypeRepository;
        private readonly Random _rnd;


        public RandomContentRecommenderBlockController(IPageCriteriaQueryService pageCriteriaQueryService, IContentTypeRepository contentTypeRepository)
        {
            _pageCriteriaQueryService = pageCriteriaQueryService;
            _contentTypeRepository = contentTypeRepository;
            _rnd = new Random();
        }

        public override ActionResult Index(Models.Blocks.RandomContentRecommenderBlock currentBlock)
        {
            if (currentBlock.CategoryList != null && currentBlock.CategoryList.Any())
            {
                var id = currentBlock.CategoryList.FirstOrDefault();
                var pages = GetPagesByCategoryId(id)
                    .OrderBy(m => m.Changed)
                    .Take(5)
                    .ToArray();

                var random = _rnd.Next(pages.Length);
                var selectedPage = pages[random] as StandardPage;

                var model = new RandomContentRecommenderViewModel
                {
                    Headline = currentBlock.Headline,
                    Heading = selectedPage.PageName,
                    Image = selectedPage.PageImage,
                    Link = selectedPage.PageLink,
                    Text = selectedPage.TeaserText
                };

                return PartialView(model);
            }

            return PartialView(new RandomContentRecommenderViewModel());
        }

        private PageDataCollection GetPagesByCategoryId(int categoryId)
        {
            var pageType = _contentTypeRepository.Load<StandardPage>();
            var pageTypeId = pageType.ID;

            return GetPagesByCategoryId(categoryId, pageTypeId);
        }

        private PageDataCollection GetPagesByCategoryId(int categoryId, int pageTypeId)
        {
           var pages = _pageCriteriaQueryService.FindAllPagesWithCriteria(
                ContentReference.RootPage, new PropertyCriteriaCollection()
                {
                    new PropertyCriteria()
                    {
                        Condition = CompareCondition.Equal,
                        Name = "PageCategory",
                        Type = PropertyDataType.Category,
                        Value = categoryId.ToString()
                    },
                    new PropertyCriteria
                    {
                        Name = "PageTypeID",
                        Type = PropertyDataType.PageType,
                        Condition = CompareCondition.Equal,
                        Value = pageTypeId.ToString(CultureInfo.InvariantCulture)
                    }
                }, null, LanguageSelector.AutoDetect(true));

            return FilterForVisitor.Filter(pages);
        }
    }
}