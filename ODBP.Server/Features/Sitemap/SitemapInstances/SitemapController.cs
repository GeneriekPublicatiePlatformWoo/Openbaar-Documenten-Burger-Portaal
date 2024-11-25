using Microsoft.AspNetCore.Mvc;

namespace ODBP.Features.Sitemap.SitemapInstances
{
    [ApiController]
    public class SitemapController
    {
        [HttpGet(ApiRoutes.Sitemap)]
        public IActionResult Get()
        {
            var model = new SitemapModel
            {
                Urls =
                [
                    new ()
                    {
                        Loc = "https://www.google.nl",
                        Document = new ()
                        {
                            DiWoo = new ()
                            {
                                Publisher = new ()
                                {
                                    Resource = "Resource",
                                    Value = "Value"
                                },
                                Titelcollectie = new ()
                                {
                                    OfficieleTitel = "Officiele titel"
                                },
                                Classificatiecollectie = new()
                                {
                                    Informatiecategorieen =
                                    [
                                        new()
                                        {
                                            Resource = "Resource",
                                            Value = "Value"
                                        }
                                    ]
                                },
                                Documenthandelingen = 
                                [
                                    new()
                                    {
                                        SoortHandeling = new()
                                        {
                                            Resource = "Resource",
                                            Value = "Value"
                                        },
                                        AtTime = "AtTime"
                                    }
                                ]
                            }
                        }
                    }
                ]
            };
            return new DiwooXmlResult(model);
        }
    }
}
