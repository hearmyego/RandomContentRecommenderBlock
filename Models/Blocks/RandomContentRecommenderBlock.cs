using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace RandomContentRecommendation.Models.Blocks
{
    [ContentType(DisplayName = "RandomContentRecommenderBlock", GUID = "8103875f-3b76-42ec-815e-9b5b926573c4", Description = "")]
    public class RandomContentRecommenderBlock : BlockData
    {
        [CultureSpecific]
        [Display(
            Name = "Headline",
            Description = "Name of the category needed to be randon extracted",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual string Headline { get; set; }


        [CultureSpecific]
        [Display(
            Name = "Category Name",
            Description = "Name of the category needed to be randon extracted",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual CategoryList CategoryList { get; set; }

    }
}